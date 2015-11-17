using UnityEngine;
using System;

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
            // new game object with registry service component
            registry_object = new GameObject("3");
            registry_object.transform.parent = transform;
            registry = registry_object.AddComponent<RegistryService>();

            // new game object with test component
            test_object = new GameObject("4");
            test_object.transform.parent = transform;
            test = test_object.AddComponent<TestComponent>();
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
            //Destroy( test_object );
            //Destroy( registry_object );
        }
    }
}