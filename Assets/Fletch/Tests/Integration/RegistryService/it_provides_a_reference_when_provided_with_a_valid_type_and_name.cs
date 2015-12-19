using UnityEngine;
using System;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_provides_a_reference_when_provided_with_a_valid_type_and_name : MonoBehaviour
    {

        GameObject registry_object;
        RegistryService registry;
        GameObject test_object;
        TestComponent test;

        // setup
        void Awake ()
        {
            registry_object = new FlexoGameObject().WithParent( gameObject ).With<RegistryService>( out registry );
            test_object = new FlexoGameObject().WithParent( gameObject ).With<TestComponent>( out test );
        }

        // test
        void Update ()
        {
            registry.Register( test.GetType(), "Foo", test );

            TestComponent result = registry.LookUp<TestComponent>( "Foo" );

            IntegrationTest.Assert( result != null, "expected to fund reference to object but got null" );
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( test_object );
            Destroy( registry_object );
        }
    }
}