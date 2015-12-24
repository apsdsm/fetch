using UnityEngine;
using Flexo;

namespace Fletch.Test {

    [IntegrationTest.DynamicTest( "Fletch.IOCServiceTest" )]
    public class it_removes_itself_from_the_ioc_list_on_destruction : MonoBehaviour {

        // setup
        void Start () {

            GameObject ioc_object = new FlexoGameObject().WithParent( gameObject ).With<IOCService>();

            IntegrationTest.Assert( ( IOC.Directory.Length == 1 ), "should be exactly 1 IOC containers before destroying the test, but found: " + IOC.Directory.Length );

            Destroy( ioc_object );

            // now give the ioc a chance to be destroyed //
        }

        // test
        void Update () {

            // ioc should have been destroyed by now //

            IntegrationTest.Assert( (IOC.Directory.Length == 0), "should be exactly 0 IOC containers after destroying the test, but found: " + IOC.Directory.Length );

            IntegrationTest.Pass();
        }

        // teardown
        void OnDisable () {
        }
    }
}