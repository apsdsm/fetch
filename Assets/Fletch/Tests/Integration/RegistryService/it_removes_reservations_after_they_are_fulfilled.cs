using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_removes_reservations_after_they_are_fulfilled : MonoBehaviour
    {

        GameObject registry_object;
        RegistryService registry;

        GameObject test_object;
        TestComponent test_component;

        TestComponent reserved_test_component;

        int timesCalledSetter = 0;

        // public setter that will be called by Registry service
        public TestComponent Bar
        {
            set {
                this.timesCalledSetter++;
                this.reserved_test_component = value;
            }
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

            // register the test component, deregister it, and register it again
            registry.Register( test_component.GetType(), "Bar", test_component );
            registry.Deregister( test_component.GetType(), "Bar" );
            registry.Register( test_component.GetType(), "Bar", test_component );

            // reservation should have been fulfilled automatically
            IntegrationTest.Assert( timesCalledSetter == 1, "setter should have been called 1 time but was called " + timesCalledSetter.ToString() + " times" );

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