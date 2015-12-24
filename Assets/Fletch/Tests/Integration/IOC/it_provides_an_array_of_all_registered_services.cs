using UnityEngine;
using Flexo;

namespace Fletch.Test
{
    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_an_array_of_all_registered_services : MonoBehaviour
    {

        GameObject ioc_object;
        IOCService ioc_service;

        // setup
        void Awake ()
        {
            ioc_object = new FlexoGameObject().WithParent( gameObject ).With<IOCService>( out ioc_service );
        }

        // test
        void Update ()
        {

            Component[] services = ioc_service.RegisteredServices();

            IntegrationTest.Assert( services != null, "should be able to get a list of services" );
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}
