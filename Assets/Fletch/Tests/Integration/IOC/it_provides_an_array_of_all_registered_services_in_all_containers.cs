using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_an_array_of_all_registered_services_in_all_containers : MonoBehaviour
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
            Component[] services = IOC.RegisteredServices();

            IntegrationTest.Assert( services.Length == 1, "should have an array of 1 services, but got: " + services.Length.ToString() );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}