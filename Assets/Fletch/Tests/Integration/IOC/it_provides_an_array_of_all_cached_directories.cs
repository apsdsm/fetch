namespace Fletch.Test.Integration.IOCTests
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_an_array_of_all_cached_directories : ioc_test
    {

        // test
        void Test ()
        {
            IntegrationTest.Assert( IOC.Directories.GetType().IsArray, "should be an array." );
            IntegrationTest.Pass();
        }
    }
}
