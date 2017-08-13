using UnityEngine;

namespace Fetch.Test.Integration.IOCTests
{
    [IntegrationTest.DynamicTest("_IOCTest")]
    public class InjectBindingTest : MonoBehaviour
    {
        // test
        void Update()
        {
            TestInjectConcrete();
            TestInjectInterface();

            IntegrationTest.Pass();
        }

        public void TestInjectInterface()
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
                    IntegrationTest.Fail("received injected instance after clearing. should have thrown exception.");
                    return;
                }
            }
            catch (NoSuchBindingException)
            {
                return;
            }
            catch
            {
                IntegrationTest.Fail("received the wrong exception");
                return;
            }

            IntegrationTest.Fail("no exception was thrown");
        }

        public void TestInjectConcrete()
        {
            var inject = new NotAddedConcrete();
            inject.id = "hogehoge";

            IOC.InjectBinding<INotAddedService>(inject);

            var result = IOC.Make<INotAddedService>();

            IntegrationTest.Assert(result == inject, "Did not return the injected instance.");

            IOC.ClearInjectedBindings();
        }
    }
}
