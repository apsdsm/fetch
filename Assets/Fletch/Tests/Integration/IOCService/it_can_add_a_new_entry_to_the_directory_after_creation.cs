using UnityEngine;
using Flexo;

namespace Fletch.Test {

    [IntegrationTest.DynamicTest( "Fletch.IOCServiceTest" )]
    public class it_can_add_a_new_entry_to_the_directory_after_creation : MonoBehaviour {

        GameObject ioc_object;
        IOCService ioc_service;
        GameObject service_object;
        FletchTestService service;
        
        // setup
        void Awake () {
            ioc_object = new FlexoGameObject( "IOC" ).With<IOCService>( out ioc_service );
        }

        // test
        void Update () {

            service_object = new FlexoGameObject( "Service" ).WithParent( ioc_object ).With<FletchTestService>( out service );

            ioc_service.AddService( typeof( IFletchTestService ), service );

            int servicesCount = ioc_service.RegisteredServices().Length;

            IntegrationTest.Assert( servicesCount == 1, "should be 1 resolvable service but found: " + servicesCount.ToString() );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable () {
            GameObject.Destroy( service_object );
            GameObject.Destroy( ioc_object );
        }
    }
}