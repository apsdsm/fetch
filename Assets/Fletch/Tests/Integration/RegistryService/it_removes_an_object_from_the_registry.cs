using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_removes_an_object_from_the_registry : registry_service_test
    {
        GameObject test_object;
        BazComponent test;

        // setup
        public override void SetUp ()
        {
            base.SetUp();

            // new game object with test component
            test_object = new FlexoGameObject( "Bar" ).WithParent( gameObject ).With<BazComponent>( out test );
        }

        // test
        void Test ()
        {
            registry.Register( test.GetType(), "TestComponent", test );

            int first_count = registry.Registrations.Length;

            IntegrationTest.Assert( first_count == 1, "expected 1 object to be registered, but found: " + first_count );

            registry.Deregister( test.GetType(), "TestComponent" );

            int second_count = registry.Registrations.Length;

            IntegrationTest.Assert( second_count == 0, "expected 0 objects to be registered, but found: " + second_count );

            IntegrationTest.Pass();

            registry.Flush();
        }

    }
}