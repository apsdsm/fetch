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
        private List<ServiceReference> services = new List<ServiceReference>();

        void Awake()
        {
            if (persistAlways) {
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Looks through each child object, and adds its interfaces to the service directory. If the
        /// object implements Fetch.IBridge, then it will add the briged class to the directory instead
        /// of the container.
        /// </summary>
        public void Populate()
        {
            foreach (Transform child in transform) {

                if (persistAlways) {
                    DontDestroyOnLoad(transform.gameObject);
                }

                foreach (Component component in child.GetComponents<Component>()) {
                    if (component != child.transform) {
                        foreach (Type type in component.GetType().GetInterfaces()) {

                            var s = new ServiceReference();

                            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IBridge<>)) {
                                s.isBridge = true;
                                s.type = type.GetGenericArguments()[0];
                            } else {
                                s.isBridge = false;
                                s.type = type;
                            }
                                                            
                            s.reference = component;
                            services.Add(s);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Return array of all services in this container.
        /// </summary>
        public ServiceReference[] Services
        {
            get {
                return services.ToArray();
            }
        }
    }
}

