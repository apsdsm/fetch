using UnityEngine;
using UnityEditor;
using System.IO;
using Flexo;

namespace Fletch.Test.Integration.IOCServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.IOCServiceTest" )]
    public class it_adds_children_as_services : ioc_service_test
    {
        
        void Test ()
        {
            // trigger manually since won't be done by the IOC
            ioc.Populate();

            IntegrationTest.Assert( ioc.Services.Length == 2, "should contain 1 service but found: " + ioc.Services.Length );
            IntegrationTest.Pass();
        }
    }
}