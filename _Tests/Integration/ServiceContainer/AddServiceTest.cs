using UnityEngine;
using System.Linq;

namespace Fetch.Test.Integration.IOCServiceTests
{
    [IntegrationTest.DynamicTest("_ServiceContainerTest")]
    public class AddServiceTest : MonoBehaviour
    {
        void Update()
        {
            // get IOC
            var container = GameObject.Find("ServiceContainerWithChildren").GetComponent<ServiceContainer>();



            // trigger manually since won't be done by the IOC static class
            container.Populate();

            TestHasFooService(container);
            TestHasBarService(container);

            IntegrationTest.Pass();
        }

        /// <summary>
        /// The Foo Service should have been added as a child of this container.
        /// </summary>
        void TestHasFooService(ServiceContainer container)
        {
            var realFoo = GameObject.Find("FooService").GetComponent<IFooService>();

            var foo = container.GetServices().First(x => x.type == typeof(IFooService));

            IntegrationTest.Assert(foo.reference == realFoo);
        }

        /// <summary>
        /// The Foo Service should have been added as a child of this container.
        /// </summary>
        void TestHasBarService(ServiceContainer container)
        {
            var realBar = GameObject.Find("BarService").GetComponent<IBarService>();

            var bar = container.GetServices().First(x => x.type == typeof(IBarService));

            IntegrationTest.Assert(bar.reference == realBar);
        }
    }
}
