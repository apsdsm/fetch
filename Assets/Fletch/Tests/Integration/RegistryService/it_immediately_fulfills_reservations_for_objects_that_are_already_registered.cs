using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_immediately_fulfills_reservations_for_objects_that_are_already_registered : registry_service_test
    {

        GameObject test_object;
        BazComponent baz_component;
        BazComponent reserved_baz_component;

        // public setter that will be called by Registry service
        public BazComponent BazComponent
        {
            set
            {
                this.reserved_baz_component = value;
            }
        }

        void Test ()
        {
            // make new object with the test component
            test_object = new FlexoGameObject().WithParent( gameObject ).With<BazComponent>( out baz_component );

            // register the test component
            registry.Register( baz_component.GetType(), "BazComponent", baz_component );

            // create new reference and make a reservation
            registry.Reserve<BazComponent>( "BazComponent", this );
            
            // reservation should have been fulfilled automatically
            IntegrationTest.Assert( reserved_baz_component != null, "reservation should have been fulfilled immediately" );
            IntegrationTest.Assert( reserved_baz_component == baz_component, "reservation should be same as test object" );
            IntegrationTest.Pass();
            registry.Flush();

        }
    }
}