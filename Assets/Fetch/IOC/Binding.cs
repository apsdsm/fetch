using System;

namespace Fetch
{
    /// <summary>
    /// A link between a query type and a concrete, instantiable type.
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// The type that is publically queried by other classes.
        /// </summary>
        public Type queryType;

        /// <summary>
        /// The type that this binding resolves to.
        /// </summary>
        public Type resolveType;

        /// <summary>
        /// True if this is a singleton binding.
        /// </summary>
        public bool singleton;

        /// <summary>
        /// If this is a singleton binding, this stores the first resovled instance.
        /// </summary>
        public object instance;
    }
}
