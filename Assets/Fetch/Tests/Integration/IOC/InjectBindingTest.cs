using UnityEngine;

namespace Fetch.Test.Integration.IOCTests
{
    [IntegrationTest.DynamicTest("_IOCTest")]
    public class InjectBindingTest : MonoBehaviour
    {
        // test
        void Update()
        {
            var inject = new NotAddedConcrete();
            inject.id = "hogehoge";

            IOC.InjectBinding<INotAddedService>(inject);

            var result = IOC.Make<INotAddedService>();
            
            IntegrationTest.Assert(result == inject, "Did not return the injected instance.");

            IOC.ClearInjectedBindings();
            
            // should not be able to get injected interface after clearing
            try
            {
                var result2 = IOC.Make<INotAddedService>();

                if (result2 == inject)
                {
                    IntegrationTest.Fail("did not clear the injected instance");
                    return;
                }
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
