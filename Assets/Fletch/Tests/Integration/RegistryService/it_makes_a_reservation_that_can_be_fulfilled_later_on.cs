using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_makes_a_reservation_that_can_be_fulfilled_later_on : registry_service_test
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
            // create new reference and make a reservation
            registry.Reserve<BazComponent>( "BazComponent", this );

            // at this point reservation should not be filled
            IntegrationTest.Assert( reserved_baz_component == null, "reserved component should be null before requested component is registered" );

            // make new object with the test component
            test_object = new FlexoGameObject().WithParent( gameObject ).With<BazComponent>( out baz_component );

            // register the test component
            registry.Register<BazComponent>( "BazComponent", baz_component );

            // reservation should have been fulfilled automatically
            IntegrationTest.Assert( reserved_baz_component != null, "reservation should have been fulfilled when requested component was registered" );
            IntegrationTest.Assert( reserved_baz_component == baz_component, "reservation should be same as test object" );
            IntegrationTest.Pass();
            registry.Flush();

        }

    }
}