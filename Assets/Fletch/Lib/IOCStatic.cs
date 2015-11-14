using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fletch {

    /// <summary>
    /// This partial contains the static methods for the IOC. I put them in 
    /// their own file because static methods are so world bendingly horrble
    /// that they need to be seperated out like this in order for the whole
    /// class not to chew its own face off. This is sad, but true.
    /// </summary>
    partial class IOC {

        /// <summary>
        /// This is a static cache of all the IOC containers. It's used so we 
        /// can reference the IOC container without having to perform any kind 
        /// of service location.
        /// </summary>
        private static List<IOC> _IOCDirectory = new List<IOC>();


        /// <summary>
        /// Returns a reference to a service that implements T.
        /// </summary>
        /// <returns>A resolved instance of type T</returns>
        public static Component Resolve<T>() {
            IOC ioc = _IOCDirectory.First();
            return ioc._Resolve<T>();
        }


        /// <summary>
        /// Provides an array of all the IOCs that are currently registered in 
        /// the directory.
        /// </summary>
        public static IOC[] Directory {
            get 
            {
                return _IOCDirectory.ToArray();
            }
        }

    }
}
