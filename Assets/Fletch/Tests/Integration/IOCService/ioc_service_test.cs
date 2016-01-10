using UnityEngine;
using TestHelpers;

namespace Fletch.Test.Integration.IOCServiceTests
{
    public class ioc_service_test : UTestCase
    {
        protected GameObject ioc_object;
        protected IOCService ioc;
        
        public override void SetUp ()
        {
            base.SetUp();

            ioc_object = GameObject.Find( "TestIOC" );
            ioc = ioc_object.GetComponent<IOCService>();
        }
    }
}
