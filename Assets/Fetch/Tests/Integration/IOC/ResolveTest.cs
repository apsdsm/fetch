using UnityEngine;

namespace Fetch.Test.Integration.IOCTests
{
    [IntegrationTest.DynamicTest("_IOCTest")]
    public class ResolveTest : MonoBehaviour
    {
        void Update()
        {
            TestResolveContainerServices();

            IntegrationTest.Pass();
        }

        /// <summary>
        /// should be able to resolve services that were spread around multiple containers
        /// </summary>
        void TestResolveContainerServices()
        {
            IFooService foo = IOC.Resolve<IFooService>();
            IntegrationTest.Assert(foo != null, "should have found a service for foo");

            IBarService bar = IOC.Resolve<IBarService>();
            IntegrationTest.Assert(bar != null, "should have found a service for bar");
        }
    }
}
