using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Fetch
{

    /// <summary>
    /// A Service Provider is a place to bind interface to concrete class types.
    /// </summary>
    public class ServiceProvider : MonoBehaviour
    {
        /// <summary>
        /// store bindings for this provider
        /// </summary>
        private List<Binding> bindings = new List<Binding>();

        /// <summary>
        /// true if provider was popualted
        /// </summary>
        private bool populated;

        /// <summary>
        /// Returns a list of bindings for this provider.
        /// </summary>
        /// <returns>list of bindings</returns>
        public List<Binding> GetBindings()
        {
            CallPopulate();
            return bindings;
        }

        /// <summary>
        /// Search for a method called 'Populate' in this class, and call it if it exists. This provides
        /// a chance for classes that derive from ServiceProvider to set their bindings.
        /// </summary>
        void CallPopulate()
        {
            if (populated)
            {
                return;
            }

            var populateMethod = GetType().GetMethod("Populate", BindingFlags.NonPublic | BindingFlags.Instance);

            if (populateMethod != null)
            {
                populateMethod.Invoke(this, new object[] { });
            }

            populated = true;
        }

        /// <summary>
        /// Bind a query type to a resolve type. Will return an instance (whenever someone asks
        /// for one of these, a new instance of the resolve type will be created and returned).
        /// </summary>
        public void Bind<Q, R>()
        {
            bindings.Add(new Binding
            {
                queryType = typeof(Q),
                resolveType = typeof(R),
                singleton = false,
            });
        }

        /// <summary>
        /// Bind a query type to a resolve type. Will be returned as a singleton (only one of the
        /// resolve type will be created, and that instance will be shared with whoever asks for it).
        /// </summary>
        public void Singleton<Q, R>()
        {
            bindings.Add(new Binding
            {
                queryType = typeof(Q),
                resolveType = typeof(R),
                singleton = true,
            });
        }
    }
}
