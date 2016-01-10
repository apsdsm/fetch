using UnityEngine;
using System;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_provides_a_list_of_all_registered_objects : registry_service_test
    {
        void Test ()
        {
            IntegrationTest.Assert( registry.Registrations is Array, "expected to find an array but found " + registry.Registrations.GetType().ToString() );
            IntegrationTest.Pass();
            registry.Flush();

        }
    }
}