using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fetch {

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
    public static class IOC {


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
        public static void Populate() {
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
        /// Provides access to currently registered services.
        /// </summary>
        public static ServiceReference[] Services {
            get {
                PopulateIfIocEmpty();
                return _ServiceCache.ToArray();
            }
        }


        /// <summary>
        /// Provides access to currently registered IOC containers.
        /// the directory.
        /// </summary>
        public static IOCService[] Directories {
            get {
                PopulateIfIocEmpty();
                return _IOCCache;
            }
        }


        /// <summary>
        /// Returns a reference to a service that implements T.
        /// </summary>
        /// <returns>A resolved instance of type T</returns>
        /// <exception cref="ServiceNotFoundException">called if service not found</exception>
        public static T Resolve<T>() {

            PopulateIfIocEmpty();

            var r = _ServiceCache.FirstOrDefault(x => x.type == typeof(T));
           
            if (r.reference == null) {
                throw new ServiceNotFoundException("could not find: " + typeof(T).ToString());
            }

            if (r.isBridge) {
                return ((IBridge<T>)r.reference).controller;
            }

            return (T)r.reference;            
        }


        /// <summary>
        /// Quickly check to see if the IOCCache is null, or if the services array 
        /// contains any nulled values (this happens when you trash a scene). If a
        /// null value is found, the services array will be populated.
        /// 
        /// This method is used as a check to make sure that Resolve isn't called
        /// on a potentially dangerous array.
        /// </summary>
        private static void PopulateIfIocEmpty() {
            if (_IOCCache != null) {
                foreach (IOCService service in _IOCCache) {
                    if (service == null) {
                        Populate();
                        return;
                    }
                }
            }
        }

    }
}
