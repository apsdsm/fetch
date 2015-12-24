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
        public static List<IOCService> _IOCDirectory = new List<IOCService>();


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
        /// Will provide a list of all service that can be resolved.
        /// </summary>
        /// <returns></returns>
        public static Type[] Services ()
        {
            List<Type> services = new List<Type>();

            foreach ( IOCService ioc in _IOCDirectory )
            {
                if ( ioc.services.Count > 0 )
                {
                    services.AddRange( ioc.services.Select( s => s.Key ) );
                }
            }

            return services.ToArray();
        }
    }
}
