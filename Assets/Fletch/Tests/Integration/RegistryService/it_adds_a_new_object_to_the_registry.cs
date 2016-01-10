using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_adds_a_new_object_to_the_registry : registry_service_test
    {

        GameObject test_object;
        BazComponent baz_component;

        public override void SetUp ()
        {
            base.SetUp();

            test_object = new FlexoGameObject( "Bar" ).WithParent( gameObject ).With<BazComponent>( out baz_component );
        }

        void Test ()
        {
            registry.Register( baz_component.GetType(), "BazComponent", baz_component );

            int length = registry.Registrations.Length;

            IntegrationTest.Assert( length == 1, "expected 1 object to be registered, but found: " + length.ToString() );
            IntegrationTest.Pass();

            registry.Flush();
        }
    }
}