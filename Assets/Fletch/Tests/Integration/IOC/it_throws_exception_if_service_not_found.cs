namespace Fletch.Test.Integration.IOCTests
{
    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_throws_exception_if_service_not_found : ioc_test
    {

        // test
        void Test ()
        {
            try
            {
                IOC.Resolve<INotAddedService>();
            }
            catch ( ServiceNotFoundException e )
            {
                IntegrationTest.Pass();
                return;
            }

            IntegrationTest.Fail( "no exception thrown" );
        }
    }
}
