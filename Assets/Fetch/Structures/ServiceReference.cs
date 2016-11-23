using System;
using System.Reflection;

namespace Fetch
{
    /// <summary>
    /// Holds a single reservation that will be fulfilled in the future.
    /// </summary>
    public struct ServiceReference
    {
        // type of object reserved
        public Type type;

        // identifier for the reservation
        public object reference;

        // true if this reference is to a bridge object
        public bool isBridge;
    }
}
