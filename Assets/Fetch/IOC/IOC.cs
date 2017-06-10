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
        private static IOCService[] iocContainers;

        /// <summary>
        /// This is a static cache of all the services inside all the IOC containers
        /// currently cached in the _IOCCache array.
        /// </summary>
        private static List<Service> services;

        /// <summary>
        /// This is a static cache of bindings that are used to make new obejcts. It is
        /// populated at scene start.
        /// </summary>
        public static List<Binding> bindings;

        /// <summary>
        /// Search for IOC containers in the scene and add them to the directory.
        /// Each container that is added will be told to populate itself and its
        /// services will be added to the cache.
        /// </summary>
        public static void Populate() {
            iocContainers = (IOCService[])GameObject.FindObjectsOfType(typeof(IOCService));
            services = new List<Service>();

            foreach (IOCService ioc in iocContainers) {
                // populate the ioc
                ioc.Populate();

                // copy its references
                foreach (Service service in ioc.Services) {
                    services.Add(service);
                }
            }
        }

        /// <summary>
        /// Provides access to currently registered services.
        /// </summary>
        public static Service[] Services {
            get {
                PopulateIfIocEmpty();
                return services.ToArray();
            }
        }

        /// <summary>
        /// Provides access to currently registered IOC containers.
        /// the directory.
        /// </summary>
        public static IOCService[] Containers {
            get {
                PopulateIfIocEmpty();
                return iocContainers;
            }
        }

        /// <summary>
        /// Returns a reference to a service that implements T.
        /// </summary>
        /// <returns>A resolved instance of type T</returns>
        /// <exception cref="ServiceNotFoundException">called if service not found</exception>
        public static T Resolve<T>() {
            PopulateIfIocEmpty();

            var r = services.FirstOrDefault(x => x.type == typeof(T));
           
            if (r.reference == null) {
                throw new ServiceNotFoundException("could not find: " + typeof(T).ToString());
            }

            if (r.isAdapter) {
                return ((IAdapter<T>)r.reference).controller;
            }

            return (T)r.reference;
        }

        /// <summary>
        /// Resolve the requested type of service or return null.
        /// </summary>
        /// <returns></returns>
        private static T ResolveOrNull<T>() {
            PopulateIfIocEmpty();

            var r = services.FirstOrDefault(x => x.type == typeof(T));
            
            if (r.reference == null) {
                return default(T);
            }

            if (r.isAdapter) {
                return ((IAdapter<T>)r.reference).controller;
            }

            return (T)r.reference;
        }

        /// <summary>
        /// Resolve the requested type of the service or return null. Does not use generics.
        /// </summary>
        /// <param name="type">The type of the service to look for</param>
        /// <returns>A non-typecast reference to the object</returns>
        private static System.Object ResolveOrNull(Type type) {
            PopulateIfIocEmpty();

            var r = services.FirstOrDefault(x => x.type == type);
            
            if (r.reference == null) {
                return null;
            }

            if (r.isAdapter) {
                return (System.Object)((IAdapter<System.Object>)r.reference).controller;
            }

            return (System.Object)r.reference;
        }

        /// <summary>
        /// Quickly check to see if the IOCCache is null, or if the services array 
        /// contains any nulled values (this happens when you trash a scene). If a
        /// null value is found, the services array will be populated.
        /// 
        /// This method is used as a check to make sure that Resolve isn't called
        /// on a potentially dangerous array.
        /// </summary>
        public static void PopulateIfIocEmpty() {
            if (iocContainers == null) {
                Populate();
                return;
            }

            foreach (IOCService service in iocContainers) {
                if (service == null) {
                    Populate();
                    return;
                }
            }
        }

        /// <summary>
        /// Dynamically make a new instance of an object at run time. Checks the parameters in the obejct's
        /// constructor and autoamtically applies any supplied parameters where appropriate. For any constructor
        /// parameters not covered by the passed parameters, Make will search for services bound to the IOC.
        /// <param name="parameters">a list of parameters you want to use to create the instance</param>
        /// <returns>an instance of the created obejct, else the default value for that object</returns>
        public static T Make<T>(params System.Object[] parameters) {

            // make new parameter array
            var ps = new Parameter[parameters.Count()];

            // assign parameters to the array
            for (var i = 0; i < parameters.Length; ++i) {
                ps[i] = new Parameter() {
                    type = parameters[i].GetType(),
                    reference = parameters[i],
                    assigned = false,
                };
            }

            // the type of thing we want to make
            var type = typeof(T);

            // see if there's a bidning that matches the type of thing we want to make
            var binding = bindings.FirstOrDefault(x => x.queryType == type);

            // get the constructors for that type
            var ctors = binding.resolveType.GetConstructors();

            // if no constructors, just make an instance and return it
            if (ctors.Count() == 0) {
                return (T)Activator.CreateInstance(binding.resolveType);
            }

            // if there are constructors, check each one, and see if there are any we can build
            foreach (var ctor in ctors) {

                // start by getting the parameters for this constructor
                var ctorParams = ctor.GetParameters();

                // make an array of equal length as the number of parameters in this constructor
                var satisfiedParams = new System.Object[ctorParams.Count()];

                // check passed parameters for passable paramters
                for (var i = 0; i < ctorParams.Count(); ++i) {
                    if (satisfiedParams[i] == null) {
                        for (var j = 0; j < ps.Length; ++j) {
                            if (ps[j].assigned == false) {
                                satisfiedParams[j] = ps[j].reference;
                                ps[j].assigned = false;
                            }
                        }
                    }
                }

                // check registered services for passable parameters
                for (var i = 0; i < ctorParams.Count(); ++i) {
                    if (satisfiedParams[i] == null) {
                        satisfiedParams[i] = ResolveOrNull(ctorParams[i].ParameterType);
                    }
                }

                // if all params weren't satisfied, keep searching
                if (satisfiedParams.Contains(null)) {
                    for (var i = 0; i < ps.Length; ++i) {
                        ps[i].assigned = false;
                    }
                    continue;
                }

                // otherwise make an instance
                var instance = Activator.CreateInstance(binding.resolveType, satisfiedParams);
                return (T)instance;
            }

            // if we couldn't make one of the thigns we wanted to make, return the default for that thing
            return default(T);
        }

        /// <summary>
        /// Bind an interface type to an implementation type. The implementation can be dynamically
        /// created at runtime using Make()
        /// </summary>
        public static void Bind<Q, R>() {
            if (bindings == null) {
                bindings = new List<Binding>();
            }

            bindings.Add(new Binding {
                queryType = typeof(Q),
                resolveType = typeof(R),
            });
        }

    }
}
