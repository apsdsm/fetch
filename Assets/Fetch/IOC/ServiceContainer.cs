using System;
using System.Collections.Generic;
using UnityEngine;

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
    public class ServiceContainer : MonoBehaviour
    {
        /// <summary>
        /// provider will persist between level loads
        /// </summary>
        public bool persistAlways;

        /// <summary>
        /// true if the service provider has been populated
        /// </summary>
        private bool populated;

        /// <summary>
        /// list of the services that are attached to this IOC Container
        /// </summary>
        private List<Service> services = new List<Service>();

        /// <summary>
        /// If the container is set to persist, will check the object's DontDestroyOnLoad flag.
        /// </summary>
        void Awake()
        {
            if (persistAlways)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Return a list of services for this container.
        /// </summary>
        /// <returns>List of services</returns>
        public List<Service> GetServices()
        {
            Populate();
            return services;
        }

        /// <summary>
        /// Looks through each component of each child object, and adds its interfaces to the service directory. 
        /// If the object implements Fetch.IProxy, then the proxied reference will be registered.
        /// </summary>
        public void Populate()
        {
            if (populated)
            {
                return;
            }

            services = new List<Service>();

            foreach (Transform child in transform)
            {
                foreach (Component component in child.GetComponents<Component>())
                {
                    if (component == child.transform)
                    {
                        continue;
                    }

                    foreach (Type type in component.GetType().GetInterfaces())
                    {
                        services.Add(MakeServiceReference(type, component));
                    }
                }
            }

            populated = true;
        }

        /// <summary>
        /// Makes a service reference object from a type and component. If component is a proxy will automatically use the proxy
        /// rather than the component.
        /// </summary>
        /// <param name="type">type of reference to be made</param>
        /// <param name="component">component holding the reference.</param>
        /// <returns>initialized service reference</returns>
        private Service MakeServiceReference(Type type, Component component)
        {
            var service = new Service();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IProxy<>))
            {
                var getProxyMethod = type.GetMethod("GetProxy");
                service.reference = getProxyMethod.Invoke(component, new object[] { });
                service.type = type.GetGenericArguments()[0];
            }
            else
            {
                service.reference = component;
                service.type = type;
            }

            return service;
        }
    }
}