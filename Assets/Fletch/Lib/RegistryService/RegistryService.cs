using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Fletch.Interfaces;
using System;

namespace Fletch
{
    public class RegistryService : MonoBehaviour, IRegistryService
    {

        // private list of registrations
        private List<Registration> registrations;


        /// <summary>
        /// Creates a new list to store registrations.
        /// </summary>
        void Awake ()
        {
            registrations = new List<Registration>();
        }


        /// <summary>
        /// Get all registrations as an array.
        /// </summary>
        public Registration[] Registrations
        {
            get
            {
                return registrations.ToArray();
            }
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
        /// Deregisters an existing object from the registry.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        public void Deregister ( Type type, string identifier )
        {
            object reference = registrations.RemoveAll( r => r.type == type && r.identifier == identifier );
        }


        /// <summary>
        /// Search for an object of specified type that matches the identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public T LookUp<T> ( string identifier )
        {
            object reference = registrations.Where( r => ( r.type == typeof( T ) ) && ( r.identifier == identifier )).FirstOrDefault<Registration>().reference;

            return (T)reference;
        }
    }
}