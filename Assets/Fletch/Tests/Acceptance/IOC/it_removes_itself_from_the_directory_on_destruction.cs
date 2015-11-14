using UnityEngine;

namespace Fletch.Test {

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_removes_itself_from_the_directory_on_destruction : MonoBehaviour {

        GameObject ioc_object;
        IOC ioc;

        // setup
        void Start () {

            // create a new IOC and attach to the test
            ioc_object = new GameObject();
            ioc_object.transform.parent = transform;
            ioc = ioc_object.AddComponent<IOC>();

            // now destroy it
            GameObject.Destroy( ioc_object );

        }

        // test
        void Update () {

            // fail if there is anything other than a single IOC in the directory
            IntegrationTest.Assert( (IOC.Directory.Length == 0), "there should be exactly 0 IOC containers in the directory, but found: " + IOC.Directory.Length );

            // otherwise pass
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable () {
            GameObject.Destroy( ioc_object );
        }
    }
}