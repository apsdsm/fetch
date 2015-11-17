using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Fletch.Interfaces;
using System;

namespace Fletch
{
    public class RegistryService : MonoBehaviour, IRegistryService
    {
        // private structure for holding registrations
        private struct Registration
        {
            public Type type;
            public string identifier;
            public object reference;
        }

        // private list of registrations
        private List<Registration> registrations;

        // set up object
        void Awake ()
        {
            registrations = new List<Registration>();
        }

        /// <summary>
        /// Count how many registrations are in the registry.
        /// </summary>
        /// <returns>Number of registrations</returns>
        public int Count ()
        {
            return registrations.Count;
        }

        /// <summary>
        /// Registers a new object in the registry.
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="identifier">Name of object</param>
        /// <param name="reference">Reference to Object</param>
        public void Register ( Type type, string identifier, object reference )
        {
            registrations.Add( new Registration() { type = type, identifier = identifier, reference = reference } );
        }

        /// <summary>
        /// Search for an object of specified type that matches the identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public T LookUp<T>( string identifier )
        {
            object reference = registrations
                                .Where( r => ( r.type == typeof( T ) ) && ( r.identifier == identifier ) )
                                .FirstOrDefault<Registration>()
                                .reference;

            return (T)reference;
        }
    }
}