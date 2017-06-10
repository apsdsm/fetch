using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fetch
{

    /// <summary>
    /// IOCService is an IOC Container that can be used to resolve references to, and
    /// create new instances of objects stored in the game scene. The container is a 
    /// game component itself, so it needs to be attached to something in the scene.
    /// 
    /// It will only resolve objects that are children of itself, and it will
    /// do a search for services when the scene starts. 
    /// </summary>
    public class IOCService : MonoBehaviour, IIocService
    {
        // IOC will persist between level loads
        public bool persistAlways = false;

        // list of the services that are attached to this IOC Container
        private List<Service> services;

        void Awake()
        {
            if (persistAlways) {
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Looks through each child object, and adds its interfaces to the service directory. If the
        /// object implements Fetch.IAdapter, then it will add the briged class to the directory instead
        /// of the container.
        /// </summary>
        public void Populate()
        {
            services = new List<Service>();

            foreach (Transform child in transform) {

                foreach (Component component in child.GetComponents<Component>()) {

                    if (component == child.transform) {
                        continue;
                    }

                    foreach (Type type in component.GetType().GetInterfaces()) {

                        // if the type is an adapter class, add as adapter reference
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IAdapter<>)) {
                            services.Add(new Service() {
                                isAdapter = true,
                                type = type.GetGenericArguments()[0],
                                reference = component,
                            });
                        
                        // otherwise add as regular reference
                        } else {
                            services.Add(new Service() {
                                isAdapter = false,
                                type = type,
                                reference = component,
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return array of all services in this container.
        /// </summary>
        public Service[] Services
        {
            get {
                if (services == null) {
                    return null;
                }

                return services.ToArray();
            }
        }
    }
}

