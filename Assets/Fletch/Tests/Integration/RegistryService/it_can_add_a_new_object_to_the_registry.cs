using UnityEngine;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_can_add_a_new_object_to_the_registry : MonoBehaviour
    {

        GameObject registry_object;
        RegistryService registry;

        GameObject test_object;
        TestComponent test;

        // setup
        void Awake ()
        {
            // new game object with registry service component
            registry_object = new GameObject("1");
            registry_object.transform.parent = transform;
            registry = registry_object.AddComponent<RegistryService>();

            // new game object with test component
            test_object = new GameObject("2");
            test_object.transform.parent = transform;
            test = test_object.AddComponent<TestComponent>();
        }

        // test
        void Update ()
        {
            registry.Register( test.GetType(), "TestComponent", test );

            int length = registry.Count();

            IntegrationTest.Assert( length == 1, "expected 1 object to be registered, but found: " + length.ToString() );
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            //Destroy( registry_object );
        }
    }
}