using UnityEngine;

namespace Fletch.Test {

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_adds_itself_to_the_directory_on_creation : MonoBehaviour {

        GameObject ioc_object;
        IOC ioc;

        // setup
        void Awake () {

            // create a new IOC and attach to the test
            ioc_object = new GameObject();
            ioc_object.transform.parent = transform;
            ioc = ioc_object.AddComponent<IOC>();

        }

        // Update is called once per frame
        void Update () {

            // fail if there is anything other than a single IOC in the directory
            IntegrationTest.Assert( (IOC.Directory.Length == 1), "there should be exactly 1 IOC container in the directory, but found: " + IOC.Directory.Length );

            // otherwise pass
            IntegrationTest.Pass();
        }

        // tear down
        void OnDisable () {
            GameObject.Destroy( ioc_object );
        }
    }
}