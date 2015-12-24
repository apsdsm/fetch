using UnityEngine;
using System;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCServiceTest" )]
    public class it_provides_an_array_of_resolvable_service_types : MonoBehaviour
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

            Type[] types = ioc_service.RegisteredServiceTypes();
            
            IntegrationTest.Assert( types != null, "should be able to get a list of services" );
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}