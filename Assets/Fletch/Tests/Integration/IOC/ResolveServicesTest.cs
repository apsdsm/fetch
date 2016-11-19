﻿using UnityEngine;

namespace Fletch.Test.Integration.IOCTests {

    [IntegrationTest.DynamicTest("_IOCTest")]
    public class ResolveServicesTest : MonoBehaviour {

        // test
        void Update() {

            IFooService foo = IOC.Resolve<IFooService>();
            IBarService bar = IOC.Resolve<IBarService>();

            IntegrationTest.Assert(foo != null, "should have found a service for foo");

            IntegrationTest.Assert(bar != null, "should have found a service for bar");

            try {
                IOC.Resolve<INotAddedService>();
            } catch (ServiceNotFoundException e) {
                IntegrationTest.Pass();
                return;
            }

            IntegrationTest.Fail("should throw exception if service not found");
        }
    }
}