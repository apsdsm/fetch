using UnityEngine;
using System.Linq;

namespace Fetch.Test.Integration.IOCServiceTests
{
    [IntegrationTest.DynamicTest("_ServiceContainerTest")]
    public class AddProxyTest : MonoBehaviour
    {
        void Update()
        {
            // get IOC
            var container = GameObject.Find("ServiceContainerWithProxy").GetComponent<ServiceContainer>();

            // trigger manually since won't be done by the IOC static class
            container.Populate();

            TestHasProxyService(container);

            IntegrationTest.Pass();
        }

        void TestHasProxyService(ServiceContainer container)
        {
            var realProxy = GameObject.Find("BazProxy").GetComponent<ProxyForBaz>().GetProxy();

            var proxy = container.GetServices().First(x => x.type == typeof(IBazService)).reference;

            IntegrationTest.Assert(proxy == realProxy);
        }
    }
}
