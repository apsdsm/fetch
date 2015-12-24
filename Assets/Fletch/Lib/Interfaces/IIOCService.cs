using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fletch.Interfaces
{
    interface IIOCService
    {
        /// <summary>
        /// Add a new service to the IOC Container. This method can be used to
        /// add services if they're created after the IOCService is instantiated in
        /// a scene.
        /// </summary>
        /// <param name="type">Type of service to add</param>
        /// <param name="service">reference to the service</param>
        void AddService ( Type type, Component service );

        /// <summary>
        /// Return a reference to the type of component specified, or null.
        /// </summary>
        /// <typeparam name="T">Type of component required</typeparam>
        /// <returns>reference to component</returns>
        Component Resolve<T>();

        /// <summary>
        /// Return an array of service references.
        /// </summary>
        /// <returns>array of component references</returns>
        Component[] RegisteredServices ();

        Type[] RegisteredServiceTypes ();
        
    }
}
