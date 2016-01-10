using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_removes_reservations_after_they_are_fulfilled : registry_service_test
    {
        GameObject test_object;
        BazComponent baz_component;
        BazComponent reserved_baz_component;

        int times_setter_called = 0;

        // public setter that will be called by Registry service
        public BazComponent BazComponent
        {
            set {
                this.times_setter_called++;
                this.reserved_baz_component = value;
            }
        }

        public override void SetUp ()
        {
            base.SetUp();

            test_object = new FlexoGameObject().WithParent( gameObject ).With<BazComponent>( out baz_component );
        }

        void Test ()
        {
            // create new reference and make a reservation
            registry.Reserve<BazComponent>( "BazComponent", this );

            // at this point reservation should not be filled
            IntegrationTest.Assert( reserved_baz_component == null, "target component should be null before requested component is registered" );

            // register the test component, deregister it, and register it again
            registry.Register( baz_component.GetType(), "BazComponent", baz_component );
            registry.Deregister( baz_component.GetType(), "BazComponent" );
            registry.Register( baz_component.GetType(), "BazComponent", baz_component );

            // reservation method should only have been called once
            IntegrationTest.Assert( times_setter_called == 1, "setter should have been called 1 time but was called " + times_setter_called.ToString() + " times" );
            IntegrationTest.Pass();
            registry.Flush();
        }
    }
}