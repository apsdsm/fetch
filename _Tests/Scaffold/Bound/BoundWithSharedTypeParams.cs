using System;

namespace Fetch.Test {
    public class BoundWithSharedTypeParams : IBoundWithSharedTypeParams
    { 
        private string a;
        private string b;

        public BoundWithSharedTypeParams(string a, string b) {
            this.a = a;
            this.b = b;
        }

        public string getA()
        {
            return this.a;
        }

        public string getB()
        {
            return this.b;
        }
    }
}
