using System;

namespace Fetch
{
    /// <summary>
    /// A link between a query type and a concrete, instantiable type.
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// The name of this object
        /// </summary>
        public string name;

        /// <summary>
        /// The type this object is registered as.
        /// </summary>
        public Type type;

        /// <summary>
        /// If this is a singleton binding, this stores the first resovled instance.
        /// </summary>
        public object instance;
    }
}
