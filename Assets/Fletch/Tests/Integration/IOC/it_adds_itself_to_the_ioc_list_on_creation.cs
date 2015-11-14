using UnityEngine;

namespace Fletch.Test {

    [IntegrationTest.DynamicTest( "Fletch.IOCTest" )]
    public class it_adds_itself_to_the_ioc_list_on_creation : MonoBehaviour {

        IOCFactory factory = new IOCFactory();

        // setup
        void Awake () {
            factory.WithNoServices();
        }

        // test
        void Update () {

            IntegrationTest.Assert( (IOC.Directory.Length == 1), "should be exactly 1 IOC container, but found: " + IOC.Directory.Length );

            IntegrationTest.Pass();
        }

        // tear down
        void OnDisable () {
            factory.TearDown();
        }
    }
}