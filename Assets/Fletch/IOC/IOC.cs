using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fletch
{

    /// <summary>
    /// This class contains the static methods for the IOC (even though static
    /// methods make babies cry). The IOC containers themselves aren't static,
    /// but the list which stores and manages them is.
    /// 
    /// By keeping this part of the process static, it allows us to poll the IOC
    /// at potentially any time.
    /// 
    /// Even though crying babies.
    /// </summary>
    public static class IOC
    {


        /// <summary>
        /// This is a static cache of all the IOC containers. It's used so we 
        /// can reference the IOC container without having to perform any kind 
        /// of service location.
        /// </summary>
        private static IOCService[] _IOCCache;


        /// <summary>
        /// This is a static cache of all the services inside all the IOC containers
        /// currently cached in the _IOCCache array.
        /// </summary>
        private static List<ServiceReference> _ServiceCache;


        /// <summary>
        /// Search for IOC containers in the scene and add them to the directory.
        /// Each container that is added will be told to populate itself and its
        /// services will be added to the cache.
        /// </summary>
        public static void Populate()
        {
            // generate new array of containers
            _IOCCache = (IOCService[])GameObject.FindObjectsOfType(typeof(IOCService));

            // clear cache and generate new list
            _ServiceCache = new List<ServiceReference>();

            foreach (IOCService ioc in _IOCCache) {
                // tell ioc to populate
                ioc.Populate();

                foreach (ServiceReference service in ioc.Services) {
                    _ServiceCache.Add(service);
                }
            }
        }

        /// <summary>
        /// Clear current IOC data and issue a new populate call.
        /// </summary>
        public static void RePopulate()
        {
            _IOCCache = null;
            _ServiceCache = null;

            IOC.Populate();
        }

        /// <summary>
        /// Returns an array containing all the services that are currently cached.
        /// </summary>
        public static ServiceReference[] Services
        {
            get {
                if (_IOCCache == null) {
                    Populate();
                }

                return _ServiceCache.ToArray();
            }
        }


        /// <summary>
        /// Provides an array of all the IOCs that are currently registered in 
        /// the directory.
        /// </summary>
        public static IOCService[] Directories
        {
            get {
                if (_IOCCache == null) {
                    Populate();
                }

                return _IOCCache;
            }
        }


        /// <summary>
        /// Returns a reference to a service that implements T.
        /// </summary>
        /// <returns>A resolved instance of type T</returns>
        public static T Resolve<T>()
        {
            if (IOCCacheIsEmpty()) {
                Populate();
            }

            T resolved = (T)_ServiceCache.FirstOrDefault(x => x.type == typeof(T)).reference;

            if (resolved == null) {
                throw new ServiceNotFoundException("could not find: " + typeof(T).ToString());
            }

            return resolved;
        }

        /// <summary>
        /// Quickly check to see if the IOCCache is empty, or if the variables it 
        /// contains have been nulled (this happens when you trash a scene)
        /// </summary>
        /// <returns></returns>
        private static bool IOCCacheIsEmpty()
        {
            if (_IOCCache == null) {
                return true;
            }

            foreach (IOCService service in _IOCCache) {
                if (service == null) {
                    return true;
                }
            }

            return false;
        }
    }
}
