using UnityEngine;

namespace Fetch.Test.Integration.IOCServiceTests {

    [IntegrationTest.DynamicTest("_IOCServiceTest")]
    public class AddAdapterTest : MonoBehaviour {

        void Update() {
            // get IOC
            var ioc = GameObject.Find("TestIOC").GetComponent<IOCService>();

            // trigger populate manually since it won't be done by the IOC static class
            ioc.Populate();

            var bridgedService = IOC.Resolve<IBazService>();

            IntegrationTest.Assert(bridgedService != null, "it should resolve controller from adapter service");
            IntegrationTest.Pass();

        }
    }
}
