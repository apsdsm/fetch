namespace Fletch.Test.Integration.IOCTests
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_caches_all_services_from_ioc_containers : ioc_test
    {

        // test
        void Test ()
        {
            int servicesCount = IOC.Services.Length;

            IntegrationTest.Assert( servicesCount == 2, "should have exactly 2 services in cache but found: " + servicesCount.ToString() );

            IntegrationTest.Pass();
        }
    }
}
