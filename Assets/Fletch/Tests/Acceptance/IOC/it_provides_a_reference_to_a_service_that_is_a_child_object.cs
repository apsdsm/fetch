using UnityEngine;

namespace Fletch.Test {

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_a_reference_to_a_service_that_is_a_child_object : MonoBehaviour {

        GameObject ioc_object;
        IOC ioc;

        GameObject service_object;
        FletchTestService service;
        
        // setup
        void Awake () {

            // create a new IOC and attach to the test
            ioc_object = new GameObject();
            ioc_object.transform.parent = transform;
            ioc = ioc_object.AddComponent<IOC>();

            // create a new dummy service and attach it to the IOC
            service_object = new GameObject();
            service_object.transform.parent = ioc_object.transform;
            service = service_object.AddComponent<FletchTestService>();

            // this would normally be called during start
            ioc.AddService( typeof( IFletchTestService ), service );
        }

        // test
        void Update () {

            // should be able to resolve the test service
            FletchTestService resolved_service = (FletchTestService)IOC.Resolve<IFletchTestService>();

            // fail if service not resolved
            IntegrationTest.Assert( resolved_service != null, "Service was not resolved." );

            // otherwise pass
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable () {
            GameObject.Destroy( service_object );
            GameObject.Destroy( ioc_object );
        }
    }
}