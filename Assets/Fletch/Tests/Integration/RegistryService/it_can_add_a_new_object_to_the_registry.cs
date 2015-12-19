using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_can_add_a_new_object_to_the_registry : MonoBehaviour
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

            int length = registry.Count();

            IntegrationTest.Assert( length == 1, "expected 1 object to be registered, but found: " + length.ToString() );
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