using System;

namespace Fetch.Test {
    public class BoundWithParams : IBoundWithParams
    { 
        private string a;
        private int b;

        public BoundWithParams(string a, int b) {
            this.a = a;
            this.b = b;
        }

        public string getA()
        {
            return this.a;
        }

        public int getB()
        {
            return this.b;
        }
    }
}
