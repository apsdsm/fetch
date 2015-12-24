using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_removes_existing_ioc_containers : MonoBehaviour
    {

        GameObject ioc_object;
        IOCService ioc_service;

        void Start ()
        {
            ioc_object = new FlexoGameObject( "IOC" ).With<IOCService>( out ioc_service );
        }

        // test
        void Update ()
        {

            IOC.DeregisterContainer( ioc_service );

            IOCService[] services = IOC.Directory;

            IntegrationTest.Assert( services.Length == 0, "should have exactly zero IOC containers but found: " + services.Length.ToString() );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}