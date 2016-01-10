using UnityEngine;
using TestHelpers;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class registry_service_test : UTestCase
    {

        protected GameObject registry_object;
        protected RegistryService registry;

        public override void SetUp ()
        {
            registry_object = GameObject.Find( "RegistryService" );
            registry = registry_object.GetComponent<RegistryService>();
        }

    }
}
