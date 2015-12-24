using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_passes_calls_to__resolve__to_an_instance : MonoBehaviour
    {

        GameObject ioc_object;
        IOCService ioc_service;
        FletchTestService test_service;

        // setup
        void Awake ()
        {
            ioc_object = new FlexoGameObject( "IOC" ).With<IOCService>( out ioc_service ).WithChild( "Foo" ).Where( "Foo" ).Has<FletchTestService>( out test_service );
            ioc_service.AddService( typeof( IFletchTestService ), test_service );
        }

        // test
        void Update ()
        {
            FletchTestService service = (FletchTestService)IOC.Resolve<IFletchTestService>();

            IntegrationTest.Assert( service != null, "should have resolved a service, but got null" );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}