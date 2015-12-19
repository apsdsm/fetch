using UnityEngine;
using System;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_a_list_of_resolvable_service_types : MonoBehaviour
    {

        GameObject ioc_object;

        // setup
        void Awake ()
        {
            ioc_object = new FlexoGameObject().WithParent( gameObject ).With<IOC>();
        }

        // test
        void Update ()
        {

            Type[] services = IOC.Services();
            
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