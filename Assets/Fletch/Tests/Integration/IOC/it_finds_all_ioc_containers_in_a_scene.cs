namespace Fletch.Test.Integration.IOCTests
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_finds_all_ioc_containers_in_a_scene : ioc_test
    {

        // test
        void Test ()
        {
            int servicesCount = IOC.Services.Length;

            IntegrationTest.Assert( IOC.Directories.Length == 2, "should have exactly 2 container but found: " + IOC.Directories.Length.ToString() );

            IntegrationTest.Pass();
        }
    }
}
