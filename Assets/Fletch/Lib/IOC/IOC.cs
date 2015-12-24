using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fletch {

    /// <summary>
    /// This partial contains the static methods for the IOC. I put them in 
    /// their own file because static methods are so world bendingly horrible
    /// that they need to be separated out like this in order for the whole
    /// class not to chew its own face off. This is sad, but true.
    /// </summary>
    public class IOC {

        /// <summary>
        /// This is a static cache of all the IOC containers. It's used so we 
        /// can reference the IOC container without having to perform any kind 
        /// of service location.
        /// </summary>
        private static List<IOCService> _IOCDirectory = new List<IOCService>();


        /// <summary>
        /// Adds a new IOC Container to a static directory of containers.
        /// </summary>
        /// <param name="service">container to add.</param>
        /// <returns>true if container was added</returns>
        public static bool RegisterContainer ( IOCService service )
        {
            if ( !IOC._IOCDirectory.Contains( service ) )
            {
                _IOCDirectory.Add( service );
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove an existing service from the directory of containers.
        /// </summary>
        /// <param name="service">service to remove</param>
        public static void DeregisterContainer ( IOCService service )
        {
            _IOCDirectory.Remove( service );
        }

        /// <summary>
        /// Returns a reference to a service that implements T.
        /// </summary>
        /// <returns>A resolved instance of type T</returns>
        public static Component Resolve<T>() {
            IOCService ioc = _IOCDirectory.First();
            return ioc.Resolve<T>();
        }


        /// <summary>
        /// Provides an array of all the IOCs that are currently registered in 
        /// the directory.
        /// </summary>
        public static IOCService[] Directory {
            get 
            {
                return _IOCDirectory.ToArray();
            }
        }


        /// <summary>
        /// Returns an instance stored in the IOC directory.
        /// </summary>
        /// <returns></returns>
        public static IOCService Instance ()
        {
            return _IOCDirectory.First();
        }

     
        /// <summary>
        /// Create an array of all services stored in all ioc containers.
        /// </summary>
        /// <returns>Array of service references</returns>
        public static Component[] RegisteredServices ()
        {
            List<Component> services = new List<Component>();

            foreach ( IOCService ioc in _IOCDirectory )
            {
                services.AddRange( ioc.RegisteredServices() );
            }

            return services.ToArray();
        }
    }
}
