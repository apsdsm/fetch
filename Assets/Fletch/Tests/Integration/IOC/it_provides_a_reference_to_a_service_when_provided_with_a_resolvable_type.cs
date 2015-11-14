using UnityEngine;
using System;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_a_reference_to_a_service_when_provided_with_a_resolvable_type : MonoBehaviour
    {

        IOCFactory factory = new IOCFactory();

        // setup
        void Awake ()
        {
            factory.WithService();
        }

        // test
        void Update ()
        {

            FletchTestService resolved = (FletchTestService)IOC.Resolve<IFletchTestService>();

            IntegrationTest.Assert( resolved != null, "should receive a reference to a service, but got null" );
            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable ()
        {
            factory.TearDown();
        }
    }
}