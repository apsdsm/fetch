using System;

namespace Fetch.Test {
    public class BoundWithServicesAndParams : IBoundWithServicesAndParams
    { 
        private string a;

        private IFooService fooService;

        public BoundWithServicesAndParams(string a, IFooService fooService) {
            this.a = a;
            this.fooService = fooService;
        }

        public IFooService GetFooService()
        {
            return this.fooService;
        }

        public string getA()
        {
            return this.a;
        }

    }
}
