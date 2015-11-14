using UnityEngine;
using UnityEditor;
using System.IO;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_adds_existing_child_services_to_directory_upon_creation : MonoBehaviour
    {

        GameObject ioc_prefab;
        GameObject ioc_instance;

        // setup
        void Awake ()
        {
            // this test involves creating a prefab, which is non-trivial
            // it would be much better if this could be encapsulated in a helper
            // class, like the factory, but more formalised.

            // create a new IOC and attach to the test
            GameObject ioc_object = new GameObject();
            ioc_object.transform.parent = transform;
            ioc_object.AddComponent<IOC>();

            // create a new dummy service and child it to the IOC
            GameObject service_object = new GameObject();
            service_object.transform.parent = ioc_object.transform;
            service_object.AddComponent<FletchTestService>();

            // create a temp directory to store a prefab
            Directory.CreateDirectory( @"Assets/__FletchTests" );

            // create the ioc prefab
            ioc_prefab = PrefabUtility.CreatePrefab( @"Assets/__FletchTests/IOC_with_children.prefab", ioc_object, ReplacePrefabOptions.Default );

            // delete the objects we created in order to create the prefab
            Destroy( ioc_object );

            // add the prefab to the game scene
            ioc_instance = PrefabUtility.InstantiatePrefab( ioc_prefab ) as GameObject;
        }

        // test
        void Update ()
        {
            int count = IOC.Services().Length;

            IntegrationTest.Assert( count == 1, "should be 1 resolvable service but found: " + count.ToString() );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_instance );
            Directory.Delete( @"Assets/__FletchTests", true );
            File.Delete( @"Assets/__FletchTests.meta" );
        }
    }
}