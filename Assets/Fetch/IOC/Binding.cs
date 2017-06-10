using System;

namespace Fetch {
    public struct Binding {

        /// <summary>
        /// The type that is publically queried by other classes.
        /// </summary>
        public Type queryType;

        /// <summary>
        /// The type that this binding resolves to.
        /// </summary>
        public Type resolveType;
    }
}
