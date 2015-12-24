
using UnityEngine;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_a_directory_of_all_ioc_containers : MonoBehaviour
    {

        GameObject ioc_object;
        IOCService ioc_service;
        FletchTestService test_service;

        // setup
        void Awake ()
        {
            ioc_object = new FlexoGameObject( "IOC" ).With<IOCService>( out ioc_service );
        }

        // test
        void Update ()
        {
            IOCService[] services = IOC.Directory;

            IntegrationTest.Assert( services.Length == 1, "should have exactly one IOC container but found: " + services.Length.ToString() );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}
