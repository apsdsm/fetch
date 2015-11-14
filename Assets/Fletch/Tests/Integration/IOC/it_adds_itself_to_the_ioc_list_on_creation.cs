using UnityEngine;

namespace Fletch.Test {

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_adds_itself_to_the_ioc_list_on_creation : MonoBehaviour {

        GameObject ioc_object;
        IOC ioc;

        // setup
        void Awake () {

            // create a new IOC and attach to the test
            ioc_object = new GameObject();
            ioc_object.transform.parent = transform;
            ioc = ioc_object.AddComponent<IOC>();

        }

        // test
        void Update () {

            IntegrationTest.Assert( (IOC.Directory.Length == 1), "should be exactly 1 IOC container, but found: " + IOC.Directory.Length );

            IntegrationTest.Pass();
        }

        // tear down
        void OnDisable () {
            GameObject.Destroy( ioc_object );
        }
    }
}