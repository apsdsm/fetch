using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fletch
{

    /// <summary>
    /// IOC is an IOC Container that can be used to resolve reference to, and
    /// crete new instances of objects stored in the game scene. The container
    /// is a game component itself, so it needs to be attached to something in
    /// the scene.
    /// 
    /// It will only resolve objects that are children of itself, and it will
    /// do a search for services when the scene starts. It will not 
    /// </summary>
    public partial class IOC : MonoBehaviour
    {

        /// <summary>
        /// Dictionary of references to services stored by the IOC.
        /// </summary>
        private Dictionary<Type, Component> services = new Dictionary<Type, Component>();


        /// <summary>
        /// Adds this object to the static directory when created.
        /// </summary>
        void Awake ()
        {
            if (!_IOCDirectory.Contains( this ))
            {
                IOC._IOCDirectory.Add( this );
                Populate();
            }
        }


        /// <summary>
        /// Removes this object from the static directory list when destroyed.
        /// </summary>
        void OnDestroy () {
            IOC._IOCDirectory.Remove( this );
        }

        /// <summary>
        /// Adds the specified service to the service directory.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="service"></param>
        public void AddService ( Type type, Component service ) 
        {
            services.Add( type, service );
        }

        /// <summary>
        /// Looks through each child object, seeking for classes that implement
        /// a interface that ends with 'Service', and adding those to the 
        /// service directory.
        /// </summary>
        private void Populate ()
        {
            foreach (Transform child in transform)
            {
                foreach (Component component in child.GetComponents<Component>())
                {
                    foreach (Type type in component.GetType().GetInterfaces())
                    {
                        if (type.ToString().EndsWith( "Service" ))
                        {
                            AddService( component.GetType(), component );
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Looks for and returns a reference to a component that implement T.
        /// </summary>
        /// <returns>A reference to a component implementing T or null</returns>
        private Component _Resolve<T> ()
        {
            try
            {
                Component service = services[ typeof( T ) ];
                return service;
            }
            catch
            {
                return null;
            }
        }
    }
}

