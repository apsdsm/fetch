using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_provides_a_reference_when_provided_with_a_valid_type_and_name : registry_service_test
    {
        GameObject test_object;
        BazComponent baz_component;

        void Update ()
        {
            test_object = new FlexoGameObject().WithParent( gameObject ).With<BazComponent>( out baz_component );

            registry.Register( baz_component.GetType(), "BazComponent", baz_component );

            BazComponent result = registry.LookUp<BazComponent>( "BazComponent" );

            IntegrationTest.Assert( result != null, "expected to fund reference to object but got null" );
            IntegrationTest.Pass();
            registry.Flush();
        }
    }
}