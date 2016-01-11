using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_can_clear_all_registered_components : registry_service_test
    {

        GameObject test_object;
        BazComponent test_component;

        public override void SetUp ()
        {
            base.SetUp();
            test_object = new FlexoGameObject( "Bar" ).WithParent( gameObject ).With<BazComponent>( out test_component );
        }

        void Test ()
        {
            registry.Register<BazComponent>( "TestComponent", test_component );

            int length = registry.Registrations.Length;

            IntegrationTest.Assert( registry.Registrations.Length == 1, "expected 1 object to be registered, but found: " + length.ToString() );

            registry.Flush();

            IntegrationTest.Assert( registry.Registrations.Length == 0, "expected 0 objects to be registered, but found: " + length.ToString() );

            IntegrationTest.Pass();
        }

    }
}