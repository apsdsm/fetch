using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_makes_a_reservation_that_can_be_fulfilled_later_on : MonoBehaviour
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
            // create new reference and make a reservation
            registry.Reserve<TestComponent>( "Bar", this );

            // at this point reservation should not be filled
            IntegrationTest.Assert( reserved_test_component == null, "reservation should be null before requested component is registered" );

            // make new object with the test component
            test_object = new FlexoGameObject().WithParent( gameObject ).With<TestComponent>( out test_component );

            // register the test component
            registry.Register( test_component.GetType(), "Bar", test_component );

            // reservation should have been fulfilled automatically
            IntegrationTest.Assert( reserved_test_component != null, "reservation should have been fulfilled when requested component was registered" );
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