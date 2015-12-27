using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_immediately_fulfills_reservations_for_objects_that_are_already_registered : MonoBehaviour
    {

        GameObject registry_object;
        RegistryService registry;

        GameObject test_object;
        TestComponent test_component;

        TestComponent reserved_test_component;

        // public setter that will be called by Registry service
        public TestComponent Bar
        {
            set { this.reserved_test_component = value; }
        }

        // setup
        void Awake ()
        {
            // new game object with registry service component
            registry_object = new FlexoGameObject( "Foo" ).WithParent( gameObject ).With<RegistryService>( out registry );
        }

        // test
        void Update ()
        {
            // make new object with the test component
            test_object = new FlexoGameObject().WithParent( gameObject ).With<TestComponent>( out test_component );

            // register the test component
            registry.Register( test_component.GetType(), "Bar", test_component );

            // create new reference and make a reservation
            registry.Reserve<TestComponent>( "Bar", this );
            
            // reservation should have been fulfilled automatically
            IntegrationTest.Assert( reserved_test_component != null, "reservation should have been fulfilled immediately" );
            IntegrationTest.Assert( reserved_test_component == test_component, "reservation should be same as test object" );
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