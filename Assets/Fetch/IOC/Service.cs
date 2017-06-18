using System;

namespace Fetch
{
    /// <summary>
    /// Holds a single reservation that will be fulfilled in the future.
    /// </summary>
    public class Service
    {
        // type of object reserved
        public Type type;

        // identifier for the reservation
        public object reference;
    }
}
