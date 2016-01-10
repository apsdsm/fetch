using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.IOCServiceTests
{
    [IntegrationTest.DynamicTest( "Fletch.IOCServiceTest" )]
    public class it_provides_an_array_of_all_registered_services : ioc_service_test
    {
    
        // test
        void Test ()
        {
            IntegrationTest.Assert( ioc.Services.GetType().IsArray, "should be an array" );
            IntegrationTest.Pass();
        }
    }
}
