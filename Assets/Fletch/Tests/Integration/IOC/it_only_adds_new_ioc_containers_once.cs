using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_only_adds_new_ioc_containers_once : MonoBehaviour
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

            IOC.RegisterContainer( ioc_service );

            IOCService[] services = IOC.Directory;

            IntegrationTest.Assert( services.Length == 1, "should have exactly 1 IOC container but found: " + services.Length.ToString() );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}
