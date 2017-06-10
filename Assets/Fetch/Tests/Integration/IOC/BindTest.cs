using UnityEngine;
using System.Linq;
using System;

namespace Fetch.Test.Integration.IOCTests {

    [IntegrationTest.DynamicTest("_IOCTest")]
    public class BindTest : MonoBehaviour {

        // test
        void Update() {
            
            IOC.Bind<IBound, Bound>();
            IOC.Bind<IBoundWithParams, BoundWithParams>();

            TestHasBindings();

            IntegrationTest.Pass();
        }

        /// <summary>
        /// The IOC should have registered each of the supplied bindings
        /// </summary>
        void TestHasBindings() {
            IntegrationTest.Assert(IocHasBinding(typeof(IBound), typeof(Bound)), "should have registered binding IBound <=> Bound");
            IntegrationTest.Assert(IocHasBinding(typeof(IBoundWithParams), typeof(BoundWithParams)), "should have registered binding IBoundWithParams <=> BoundWithParams");
        }

        /// <summary>
        /// True if the IOC has a binding with the specified attributes
        /// </summary>
        /// <param name="queryType">query type of binding</param>
        /// <param name="resolveType">resolve type of binding</param>
        /// <returns>true or false</returns>
        bool IocHasBinding(Type queryType, Type resolveType) {
            return IOC.bindings.Any(x => x.queryType == queryType && x.resolveType == resolveType);
        }
    }
}
