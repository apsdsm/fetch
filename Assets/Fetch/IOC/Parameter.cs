using System;

namespace Fetch
{
    /// <summary>
    /// A representation of a parameter in an obejct constructor.
    /// </summary>
    public class Parameter
    {
        // type of object reserved
        public Type type;

        // identifier for the reservation
        public object reference;

        // true if this parameter has been assigned
        public bool assigned;
    }
}
