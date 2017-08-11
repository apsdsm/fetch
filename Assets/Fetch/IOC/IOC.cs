using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Fetch
{

    /// <summary>
    /// This class contains the static methods for the IOC.
    /// </summary>
    public static class IOC
    {

        /// <summary>
        /// This is a static cache of all the services inside all the IOC containers
        /// currently cached in the _IOCCache array.
        /// </summary>
        static List<Service> services;

        /// <summary>
        /// This is a static cache of bindings that are used to make new obejcts. It is
        /// populated at scene start.
        /// </summary>
        static List<Binding> bindings;

        /// <summary>
        /// This is a static cache of injected bindings. This can be used to return mock data for testing, or for
        /// injecting an instantiated biding at run time that will override previous bindings.
        /// </summary>
        private static List<Binding> injected;
        
        /// <summary>
        /// True if the IOC has been populated for this scene. It should be set to false when the scene changes.
        /// </summary>
        static bool populated;

        /// <summary>
        /// True if the scene change events have been registered. These events are used to keep track of when the bindings need to be refreshed.
        /// </summary>
        static bool eventsRegistered;

        private static void OnActiveSceneChanged(Scene i_preChangedScene, Scene i_postChangedScene)
        {
            populated = false;
            Debug.LogFormat("OnActiveSceneChanged() preChangedScene:{0} postChangedScene:{1}", i_preChangedScene.name, i_postChangedScene.name);
        }

        private static void OnSceneLoaded(Scene i_loadedScene, LoadSceneMode i_mode)
        {
            populated = false;
            Debug.LogFormat("OnSceneLoaded() current:{0} loadedScene:{1} mode:{2}", SceneManager.GetActiveScene().name, i_loadedScene.name, i_mode);
        }

        /// <summary>
        /// Search for IOC containers in the scene and add them to the directory.
        /// Each container that is added will be told to populate itself and its
        /// services will be added to the cache.
        /// </summary>
        public static void Populate()
        {
            if (populated)
            {
                return;
            }

            if (!eventsRegistered)
            {
                SceneManager.activeSceneChanged += OnActiveSceneChanged;
                SceneManager.sceneLoaded += OnSceneLoaded;
                eventsRegistered = true;
            }

            var serviceContainers = (ServiceContainer[])GameObject.FindObjectsOfType(typeof(ServiceContainer));

            services = new List<Service>();

            foreach (var container in serviceContainers)
            {
                services = services.Union(container.GetServices()).ToList();
            }

            bindings = new List<Binding>();

            var serviceProviders = GameObject.FindObjectsOfType<ServiceProvider>();

            foreach (var provider in serviceProviders)
            {
                bindings = bindings.Union(provider.GetBindings()).ToList();
            }

            populated = true;
        }

        /// <summary>
        /// Returns a reference to the service that implements the given type, or throws an error.
        /// </summary>
        /// <param name="type">Type to search for</param>
        /// <returns>An object that implements the given type</returns>
        public static System.Object Resolve(Type type)
        {
            Populate();
            var resolved = services.FirstOrDefault(x => x.type == type);

            if (resolved == null)
            {
                throw new ServiceNotFoundException("could not find: " + type.ToString());
            }

            return resolved.reference;
        }

        /// <summary>
        /// Alternate syntax for Resolve(Type).
        /// </summary>
        /// <returns>A resolved instance of type T</returns>
        /// <exception cref="ServiceNotFoundException">called if service not found</exception>
        public static T Resolve<T>()
        {
            Populate();
            var resolved = services.FirstOrDefault(x => x.type == typeof(T));

            if (resolved == null)
            {
                throw new ServiceNotFoundException("could not find: " + typeof(T).ToString());
            }

            return (T)resolved.reference;
        }

        /// <summary>
        /// Alterate syntax for ResolveOrNull(Type).
        /// </summary>
        /// <returns></returns>
        public static T ResolveOrNull<T>()
        {
            return (T)ResolveOrNull(typeof(T));
        }

        /// <summary>
        /// Resolve the requested type of the service or return null. Does not use generics.
        /// </summary>
        /// <param name="type">The type of the service to look for</param>
        /// <returns>A non-typecast reference to the object</returns>
        public static System.Object ResolveOrNull(Type type)
        {
            Populate();
            var service = services.FirstOrDefault(x => x.type == type);
            if (service == null)
            {
                return null;
            }
            return service.reference;
        }

        /// <summary>
        /// Generic version of Make(Type, System.Object[]).
        /// </summary>
        /// <param name="parameters">a list of parameters you want to use to create the instance</param>
        /// <returns>an instance of the created object, else the default value for that object</returns>
        public static T Make<T>(params System.Object[] parameters)
        {
            return (T)Make(typeof(T), parameters);
        }

        /// <summary>
        /// Dynamically make a new instance of an object at run time. Checks the parameters in the obejct's
        /// constructor and autoamtically applies any supplied parameters where appropriate. For any constructor
        /// parameters not covered by the passed parameters, Make will search for services bound to the IOC.
        /// </summary>
        /// <param name="type">type of object to build</param>
        /// <param name="parameters">a list of parameters you want to use to create the instance</param>
        /// <returns>reference to build object</returns>
        public static System.Object Make(Type type, params System.Object[] parameters)
        {
            Populate();

            Binding binding;           
            
            // if there are any injected bindings that match, return that
            if (injected != null && injected.Count >= 1 && (binding = injected.FirstOrDefault(x => x.queryType == type)) != null)
            {
                return binding.instance;
            }
            
            // try to make a new binding from 
            if (bindings == null || (binding = bindings.FirstOrDefault(x => x.queryType == type)) == null)
            {
                if (type.IsInterface || type.IsAbstract)
                {
                    throw new NoSuchBindingException("Cannot late bind interfaces or abstract classes.");
                }
                
                binding = new Binding();
                binding.queryType = type;
                binding.resolveType = type;
                binding.singleton = false;
            }

            var ctors = binding.resolveType.GetConstructors();

            // if it's a singleton that has previously been resolved, return that
            if (binding.singleton && binding.instance != null)
            {
                return binding.instance;
            }

            // if class has no constructors, make it without searching for parameters
            if (ctors.Length == 0)
            {
                return Activator.CreateInstance(binding.resolveType);
            }

            //put the supplied parameters into an array that makes them easier to work with
            var suppliedParams = new Parameter[parameters.Count()];

            for (var i = 0; i < parameters.Length; ++i)
            {
                suppliedParams[i] = new Parameter()
                {
                    type = parameters[i].GetType(),
                    reference = parameters[i],
                    assigned = false,
                };
            }

            // if there are constructors, check each one, and return the first one we can build
            foreach (var ctor in ctors)
            {
                var ctorParams = ctor.GetParameters();
                var paramCount = ctorParams.Length;
                var filledParams = new System.Object[paramCount];

                // check passed parameters for passable paramters
                for (var i = 0; i < paramCount; ++i)
                {
                    if (filledParams[i] == null)
                    {
                        for (var j = 0; j < suppliedParams.Length; ++j)
                        {
                            if (suppliedParams[j].assigned == false && suppliedParams[j].type == ctorParams[i].ParameterType)
                            {
                                filledParams[i] = suppliedParams[j].reference;
                                suppliedParams[j].assigned = true;
                                break;
                            }
                        }
                    }
                }

                // check registered services for passable parameters
                for (var i = 0; i < paramCount; ++i)
                {
                    if (filledParams[i] == null)
                    {
                        filledParams[i] = ResolveOrNull(ctorParams[i].ParameterType);
                    }
                }

                // check to see if there are any parameters that can be created using Make
                for (var i = 0; i < paramCount; ++i)
                {
                    if (filledParams[i] == null)
                    {
                        var parameterBinding = bindings.FirstOrDefault(x => x.queryType == ctorParams[i].ParameterType);

                        if (parameterBinding != null)
                        {
                            filledParams[i] = Make(parameterBinding.queryType, parameters);
                        }
                    }
                }

                // if all params were filled make intance and pass it back
                if (!filledParams.Contains(null))
                {
                    var instance = Activator.CreateInstance(binding.resolveType, filledParams);

                    if (binding.singleton)
                    {
                        binding.instance = instance;
                    }

                    return instance;
                }

                // reset the suppliedParameters array
                for (var i = 0; i < suppliedParams.Length; ++i)
                {
                    suppliedParams[i].assigned = false;
                }
            }

            // if we couldn't make one of the thigns we wanted to make, return the default for that thing
            return null;
        }

        /// <summary>
        /// Inject an instance of an object that should be returned when the user queries for a binding of type T.
        /// This can conceivably be used for late binding at runtime, but it's intended purpose is to allow for easier
        /// testing of classes that rely on hard-to-reach IOC calls.
        /// 
        /// After using InjectBiding, restore the IOC to its previous state using 'ClearInjectedBindings'.
        /// </summary>
        /// <param name="instance">A concrete instance of the object to be returned</param>
        /// <typeparam name="T">The type of object that will be returned</typeparam>
        public static void InjectBinding<T>(T instance)
        {
            if (injected == null)
            {
                injected = new List<Binding>();
            }
            
            var binding = new Binding();
            binding.instance = instance;
            binding.queryType = typeof(T);
            binding.resolveType = typeof(T);
            
            injected.Add(binding);
        }

        /// <summary>
        /// Clears any injected bindings, returning the cache to null. This is not done automatically, so be aware that
        /// if you spam the InjectedBindings method the objects that were bound won't be properly garbage collected.
        /// </summary>
        public static void ClearInjectedBindings()
        {
            injected = null;
        }
    }
}
