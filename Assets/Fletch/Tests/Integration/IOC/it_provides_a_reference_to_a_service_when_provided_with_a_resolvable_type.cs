using UnityEngine;
using System;
using Flexo;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_a_reference_to_a_service_when_provided_with_a_resolvable_type : MonoBehaviour
    {
        GameObject ioc_object;
        IOCService ioc;
        FletchTestService service;

        // setup
        void Awake ()
        {
            // new IOC object with service
            ioc_object = new FlexoGameObject().WithParent( gameObject ).With<IOCService>( out ioc ).WithChild( "Foo" ).Where( "Foo" ).Has<FletchTestService>( out service );

            // add service
            ioc.AddService( typeof( IFletchTestService ), service );
        }

        // test
        void Update ()
        {
            // resolve service back from ioc
            FletchTestService resolved = (FletchTestService)IOC.Resolve<IFletchTestService>();

            IntegrationTest.Assert( resolved != null, "should receive a reference to a service, but got null" );
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            Destroy( ioc_object );
        }
    }
}