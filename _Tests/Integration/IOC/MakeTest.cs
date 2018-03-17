using UnityEngine;

namespace Fetch.Test.Integration.IOCTests
{

    [IntegrationTest.DynamicTest("_IOCTest")]
    public class MakeTest : MonoBehaviour
    {
        private IFooService fooService;

        // test
        void Update()
        {
            // get foo servicer reference
            fooService = IOC.Resolve<IFooService>();

            TestMakeWithNoParams();
            TestMakeWithSharedTypeParams();
            TestMakeWithServices();
            TestMakeWithParams();
            TestMakeWithServicesAndParams();
            TestMakeSingleton();
            TestMakeServiceThatRequiresOtherBoundServices();
            TestMakeLateBoundClassWithServices();
            
            IntegrationTest.Pass();
        }

        /// <summary>
        /// The Make method should be able to make an isntance of an object that has no parameters.
        /// </summary>
        void TestMakeWithNoParams()
        {
            var instance = IOC.Make<IBound>();
            IntegrationTest.Assert(instance != null, "should have made an instance of Bound");
        }

        /// <summary>
        /// The Make method should be able to make an instance of an object that references services in the IOC.
        /// </summary>
        void TestMakeWithServices()
        {
            var instance = IOC.Make<IBoundWithServices>();
            IntegrationTest.Assert(instance != null, "should have have instance of BoundWithServices");
            IntegrationTest.Assert(instance.GetFooService() == fooService, "should have passed foo service registered in IOC");
        }

        /// <summary>
        /// The Make method should be able to make an instance of an object that passes in the parameters it wants fed to the constructor.
        /// </summary>
        void TestMakeWithParams()
        {
            var instance2 = IOC.Make<IBoundWithParams>("foobar", 123);
            IntegrationTest.Assert(instance2 != null, "should have made an instance of BoundWithParams");
            IntegrationTest.Assert(instance2.getA() == "foobar" && instance2.getB() == 123, "should have passed parameters.");
        }

        /// <summary>
        /// The Make method should be able to make an instance of an object that passes in the parameters it wants fed to the constructor.
        /// </summary>
        void TestMakeWithSharedTypeParams()
        {
            var instance = IOC.Make<IBoundWithSharedTypeParams>("foobar", "barbax");
            IntegrationTest.Assert(instance != null, "should have made an instance of BoundWithSharedTypeParams");
            IntegrationTest.Assert(instance.getA() == "foobar" && instance.getB() == "barbax", "should have passed parameters but got <" + instance.getA() + ", " + instance.getB() + ">");
        }

        /// <summary>
        /// The Make method should be able to make an instance of an object using local parameters, filling in the gaps with registered services.
        /// </summary>
        void TestMakeWithServicesAndParams()
        {
            var instance = IOC.Make<IBoundWithServicesAndParams>("foobar");
            IntegrationTest.Assert(instance.getA() == "foobar", "should have passed parameters but got <" + instance.getA() + ">");
            IntegrationTest.Assert(instance.GetFooService() == fooService, "should have passed foo service registered in IOC");
        }

        /// <summary>
        /// The Make method should be able to make a single just one time, then return the previously make instance on subsequent calls.
        /// </summary>
        void TestMakeSingleton()
        {
            var instance1 = IOC.Make<IBazService>();
            var instance2 = IOC.Make<IBazService>();
            IntegrationTest.Assert(instance1 == instance2, "should have two references to same object.");
        }

        /// <summary>
        /// the Make method should be able to make an instance of an object whose constructor calls for other objects that are also bound objects.
        /// </summary>
        void TestMakeServiceThatRequiresOtherBoundServices()
        {
            var instance = IOC.Make<IBoundWithOtherBoundServices>();
            var bazSingleton = IOC.Make<IBazService>();
            IntegrationTest.Assert(instance != null, "did not make recursive bound object");
            IntegrationTest.Assert(instance.GetBazService() == bazSingleton);
        }

        /// <summary>
        /// the Make method should be able to make a concrete object that wasn't bound, so long as the constructor paramters are made up of bound or passed objects.e
        /// </summary>
        void TestMakeLateBoundClassWithServices()
        {
            var instance = IOC.Make<NonBoundWithServices>();
            IntegrationTest.Assert(instance != null, "should have made instance of NonBoundWithServices");
            IntegrationTest.Assert(instance.GetFooService() == fooService, "should have passed foo service registered in IOC");
        }
    }
}
