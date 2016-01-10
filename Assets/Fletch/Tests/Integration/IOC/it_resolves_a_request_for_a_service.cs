namespace Fletch.Test.Integration.IOCTests
{
    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_resolves_a_request_for_a_service : ioc_test
    {

        // test
        void Test ()
        {
            IFooService foo = IOC.Resolve<IFooService>();
            IBarService bar = IOC.Resolve<IBarService>();

            IntegrationTest.Assert( foo != null, "should have found a service for foo" );
            IntegrationTest.Assert( bar != null, "should have found a service for bar" );
            IntegrationTest.Pass();
        }
    }
}
