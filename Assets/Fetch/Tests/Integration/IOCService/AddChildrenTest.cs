﻿using UnityEngine;

namespace Fetch.Test.Integration.IOCServiceTests {

    [IntegrationTest.DynamicTest("_IOCServiceTest")]
    public class AddChildrenTest : MonoBehaviour {

        void Update() {

            // get IOC
            var ioc = GameObject.Find("TestIOC").GetComponent<IOCService>();

            // trigger manually since won't be done by the IOC static class
            ioc.Populate();

            IntegrationTest.Assert(ioc.Services.Length == 3, "should contain 3 services but found: " + ioc.Services.Length);
            IntegrationTest.Pass();
        }
    }
}