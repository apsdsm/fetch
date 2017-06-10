using UnityEngine;

namespace Fetch.Test.Integration.IOCTests {

    [IntegrationTest.DynamicTest("_IOCTest")]
    public class MakeTest : MonoBehaviour {

        private IFooService fooService;

        // test
        void Update() {
            IOC.Bind<IBound, Bound>();
            IOC.Bind<IBoundWithServices, BoundWithServices>();
            IOC.Bind<IBoundWithParams, BoundWithParams>();
            IOC.Bind<IBoundWithSharedTypeParams, BoundWithSharedTypeParams>();
            IOC.Bind<IBoundWithServicesAndParams, BoundWithServicesAndParams>();

            fooService = IOC.Resolve<IFooService>();

            TestMakeWithNoParams();
            TestMakeWithSharedTypeParams();
            TestMakeWithServices();
            TestMakeWithParams();
            TestMakeWithServicesAndParams();

            IntegrationTest.Pass();
        }

        /// <summary>
        /// The Make method should be able to make an isntance of an object that has no parameters.
        /// </summary>
        void TestMakeWithNoParams(){
            var instance = IOC.Make<IBound>();
            IntegrationTest.Assert(instance != null, "should have made an instance of Bound");
        }

        /// <summary>
        /// The Make method should be able to make an instance of an object that references services in the IOC.
        /// </summary>
        void TestMakeWithServices(){
            var instance = IOC.Make<IBoundWithServices>();
            IntegrationTest.Assert(instance != null, "should have have instance of BoundWithServices");
            IntegrationTest.Assert(instance.GetFooService() == fooService, "should have passed foo service registered in IOC");
        }

        /// <summary>
        /// The Make method should be able to make an instance of an object that passes in the parameters it wants fed to the constructor.
        /// </summary>
        void TestMakeWithParams() {
            var instance2 = IOC.Make<IBoundWithParams>("foobar", 123);
            IntegrationTest.Assert(instance2 != null, "should have made an instance of BoundWithParams");
            IntegrationTest.Assert(instance2.getA() == "foobar" && instance2.getB() == 123, "should have passed parameters.");
        }

        /// <summary>
        /// The Make method should be able to make an instance of an object that passes in the parameters it wants fed to the constructor.
        /// </summary>
        void TestMakeWithSharedTypeParams() {
            var instance = IOC.Make<IBoundWithSharedTypeParams>("foobar", "barbax");
            IntegrationTest.Assert(instance != null, "should have made an instance of BoundWithSharedTypeParams");
            IntegrationTest.Assert(instance.getA() == "foobar" && instance.getB() == "barbax", "should have passed parameters but got <" + instance.getA() + ", " + instance.getB() + ">");
        }

        /// <summary>
        /// The Make method should be able to make an instance of an object using local parameters, filling in the gaps with registered services.
        /// </summary>
        void TestMakeWithServicesAndParams() {
            var instance = IOC.Make<IBoundWithServicesAndParams>("foobar");
            IntegrationTest.Assert(instance.getA() == "foobar", "should have passed parameters but got <" + instance.getA() + ">");
            IntegrationTest.Assert(instance.GetFooService() == fooService, "should have passed foo service registered in IOC");
        }
    }
}
