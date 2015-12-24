using UnityEngine;
using UnityEditor;
using System.IO;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_an_iocservice_instance : MonoBehaviour
    {

        GameObject ioc_object;

        // setup
        void Awake ()
        {
            ioc_object = new FlexoGameObject( "IOC" ).With<IOCService>();
        }

        // test
        void Update ()
        {
            IOCService service = IOC.Instance();

            IntegrationTest.Assert( service != null, "should provide instance of IOC Service but got null." );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}