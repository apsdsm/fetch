﻿using System;

namespace Fletch
{
    /// <summary>
    /// Provides a registry that objects can register themselves with. This allows
    /// objects to talk to get references to one another even when they're not 
    /// directly interacting.  
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        /// Registers a single object in the registry
        /// </summary>
        /// <param name="type">the type of the object</param>
        /// <param name="identifier">the identifier that it will be known by</param>
        /// <param name="reference">a reference to the object</param>
        void Register ( Type type, string identifier, object reference );


        /// <summary>
        /// Deregister a single object in the registry
        /// </summary>
        /// <param name="type">the type identifier</param>
        /// <param name="identifier">the string identifier</param>
        void Deregister ( Type type, string identifier );


        /// <summary>
        /// Return a reference currently stored in the registry.
        /// </summary>
        /// <typeparam name="T">The type of reference to return</typeparam>
        /// <param name="identifier">The identifier assigned to the reference</param>
        /// <returns>a reference to a generic object that matches both search criteria</returns>
        T LookUp<T>( string identifier );


        /// <summary>
        /// Provide a getter that returns an array of all the current registrations.
        /// </summary>
        Registration[] Registrations
        { get; }
    }
}