using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_throws_an_exception_if_setter_is_not_found_for_reservation : MonoBehaviour
    {

        GameObject registry_object;
        RegistryService registry;


        // setup
        void Awake ()
        {
            // new game object with registry service component
            registry_object = new FlexoGameObject( "Foo" ).WithParent( gameObject ).With<RegistryService>( out registry );
        }

        // test
        void Update ()
        {
            try
            {
                registry.Reserve<TestComponent>( "NotValid", this );

            }
            catch ( SetterNotFoundException e )
            {
                IntegrationTest.Assert( e.Message == "no setter was found with the name NotValid", "wrong message in exception: " + e.Message );
                IntegrationTest.Pass();
            }            
        }

        // teardown
        void OnDisable ()
        {
            Destroy( registry_object );
        }
    }
}