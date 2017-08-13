using UnityEngine;

namespace Fetch.Test.Integration.IOCTests
{
    [IntegrationTest.DynamicTest("_IOCTest")]
    public class MakeExceptionTest : MonoBehaviour
    {
        void Update()
        {
            // should not be able to late bind an interface
            try
            {
                IOC.Make<INotAddedService>();
            }
            catch (NoSuchBindingException)
            {
                IntegrationTest.Pass();
                return;
            }
            catch
            {
                IntegrationTest.Fail("received the wrong exception");
                return;
            }

            IntegrationTest.Fail("no exception was thrown");
        }
    }
}
