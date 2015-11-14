using UnityEngine;

namespace Fletch.Test {

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_can_add_a_new_service_to_the_directory_after_creation : MonoBehaviour {

        GameObject ioc_object;
        IOC ioc;
        GameObject service_object;
        FletchTestService service;
        
        // setup
        void Awake () {

            // This test involves calling a method on the ioc, so we won't use
            // the factory in this instance, and do the creation by hand - that
            // said I wish there was a way to automate this kinf of thing too.

            // create a new IOC and attach to the test
            ioc_object = new GameObject();
            ioc_object.transform.parent = transform;
            ioc = ioc_object.AddComponent<IOC>();

            // create a new dummy service and attach it to the IOC
            service_object = new GameObject();
            service_object.transform.parent = ioc_object.transform;
            service = service_object.AddComponent<FletchTestService>();

            // Add the service as a new child
            ioc.AddService( typeof( IFletchTestService ), service );
            
        }

        // test
        void Update () {

            int count = IOC.Services().Length;

            IntegrationTest.Assert( count == 1, "should be 1 resolvable service but found: " + count.ToString() );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable () {
            GameObject.Destroy( service_object );
            GameObject.Destroy( ioc_object );
        }
    }
}