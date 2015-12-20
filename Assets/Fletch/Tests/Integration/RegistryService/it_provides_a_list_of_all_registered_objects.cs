using UnityEngine;
using System;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_provides_a_list_of_all_registered_objects : MonoBehaviour
    {

        GameObject registry_object;
        RegistryService registry;

        // setup
        void Awake ()
        {
            registry_object = new FlexoGameObject().WithParent( gameObject ).With<RegistryService>( out registry );
        }

        // test
        void Update ()
        {
            Registration[] registrations = registry.Registrations;

            IntegrationTest.Assert( (registrations is Array ), "expected to find an array but found " + registrations.GetType().ToString() );
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( registry_object );
        }
    }
}