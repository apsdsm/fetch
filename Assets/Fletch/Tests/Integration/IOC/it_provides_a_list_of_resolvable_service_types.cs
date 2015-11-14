using UnityEngine;
using System;

namespace Fletch.Test
{

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_provides_a_list_of_resolvable_service_types : MonoBehaviour
    {

        IOCFactory factory = new IOCFactory();

        // setup
        void Awake ()
        {
            factory.WithNoServices();
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
            factory.TearDown();
        }
    }
}