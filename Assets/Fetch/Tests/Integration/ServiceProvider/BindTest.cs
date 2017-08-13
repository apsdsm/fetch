using UnityEngine;
using System.Linq;
using System;

namespace Fetch.Test.Integration.ServiceProviderTests
{

    [IntegrationTest.DynamicTest("_ServiceProviderTest")]
    public class BindTest : MonoBehaviour
    {

        private DummyServiceProvider services;

        // test
        void Update()
        {

            // get ServiceProvider
            services = GameObject.Find("DummyServiceProvider").GetComponent<DummyServiceProvider>();

            TestHasBindings();
            TestHasSingletons();

            IntegrationTest.Pass();
        }

        // The dummy service provider should have registered each of the binding during its Awake phase
        void TestHasBindings()
        {
            IntegrationTest.Assert(HasBinding(typeof(IBound), typeof(Bound)), "should have registered binding IBound <=> Bound");
            IntegrationTest.Assert(HasBinding(typeof(IBoundWithParams), typeof(BoundWithParams)), "should have registered binding IBoundWithParams <=> BoundWithParams");
        }

        // The dummy service provider should have registered a singleton service.
        void TestHasSingletons()
        {
            IntegrationTest.Assert(HasSingletonBinding(typeof(IBazService), typeof(BazService)), "should have registered singleton binding IBazService <=> BazService");
        }

        // True if service has the specified binding
        bool HasBinding(Type queryType, Type resolveType)
        {
            return services.GetBindings().Any(x => x.queryType == queryType && x.resolveType == resolveType && x.singleton == false);
        }

        // True if the service has the specified binding as a singleton
        bool HasSingletonBinding(Type queryType, Type resolveType)
        {
            return services.GetBindings().Any(x => x.queryType == queryType && x.resolveType == resolveType && x.singleton == true);
        }
    }
}
