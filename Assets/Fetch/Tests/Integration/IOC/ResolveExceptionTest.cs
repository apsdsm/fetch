using UnityEngine;

namespace Fetch.Test.Integration.IOCTests
{
    [IntegrationTest.DynamicTest("_IOCTest")]
    public class ResolveExceptionTest : MonoBehaviour
    {
        void Update()
        {
            // should throw error to pass
            try
            {
                IOC.Resolve<INotAddedService>();
            }
            catch (ServiceNotFoundException)
            {
                IntegrationTest.Pass();
                return;
            }
            catch
            {
                IntegrationTest.Fail("received the wrong exception");
                return;
            }

            IntegrationTest.Fail("should throw exception if service not found");
        }
    }
}
