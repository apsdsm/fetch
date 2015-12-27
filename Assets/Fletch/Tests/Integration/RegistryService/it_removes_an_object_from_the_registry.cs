using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_removes_an_object_from_the_registry : MonoBehaviour
    {

        GameObject registry_object;
        RegistryService registry;
        GameObject test_object;
        TestComponent test;

        // setup
        void Awake ()
        {
            // new game object with registry service component
            registry_object = new FlexoGameObject( "Foo" ).WithParent( gameObject ).With<RegistryService>( out registry );

            // new game object with test component
            test_object = new FlexoGameObject( "Bar" ).WithParent( gameObject ).With<TestComponent>( out test );
        }

        // test
        void Update ()
        {
            registry.Register( test.GetType(), "TestComponent", test );

            int first_count = registry.Registrations.Length;

            IntegrationTest.Assert( first_count == 1, "expected 1 object to be registered, but found: " + first_count );

            registry.Deregister( test.GetType(), "TestComponent" );

            int second_count = registry.Registrations.Length;

            IntegrationTest.Assert( second_count == 0, "expected 0 objects to be registered, but found: " + second_count );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( registry_object );
            Destroy( test_object );
        }
    }
}