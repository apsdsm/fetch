namespace Fletch.Test.Integration.IOCTests
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_an_array_of_all_cached_services : ioc_test
    {

        // test
        void Test ()
        {
            IntegrationTest.Assert( IOC.Services.GetType().IsArray, "should be an array." );
            IntegrationTest.Pass();
        }
    }
}
