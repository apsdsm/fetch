using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fletch
{

    /// <summary>
    /// IOC is an IOC Container that can be used to resolve reference to, and
    /// create new instances of objects stored in the game scene. The container
    /// is a game component itself, so it needs to be attached to something in
    /// the scene.
    /// 
    /// It will only resolve objects that are children of itself, and it will
    /// do a search for services when the scene starts. 
    /// </summary>
    public class IOCService : MonoBehaviour, IIOCService
    {

        // list of the services that are attached to this IOC Container
        private List<ServiceReference> services = new List<ServiceReference>();

        /// <summary>
        /// Looks through each child object, seeking for classes that implement
        /// a interface that ends with one of the keywords: 'Service', 'Factory',
        /// 'Manager', or 'Controller, and adding those to the service directory.
        /// </summary>
        public void Populate ()
        {
            foreach (Transform child in transform)
            {
                foreach (Component component in child.GetComponents<Component>())
                {
                    foreach (Type type in component.GetType().GetInterfaces())
                    {
                        string typeString = type.ToString();

                        if (typeString.EndsWith( "Service" ) || 
                            typeString.EndsWith( "Factory" ) || 
                            typeString.EndsWith( "Manager") ||
                            typeString.EndsWith( "Controller" ) )
                        {
                            ServiceReference serviceReference = new ServiceReference();
                            serviceReference.type = type;
                            serviceReference.reference = component;

                            services.Add( serviceReference );
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
            get
            {
                return services.ToArray();
            }
        }
    }
}

