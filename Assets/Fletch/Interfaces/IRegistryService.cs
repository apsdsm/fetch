using System;

namespace Fletch.Interfaces
{
    /// <summary>
    /// Provides a registry that objects can register themselves with. This allows
    /// objects to talk to get references to one another even when they're not 
    /// directly interacting.  
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        /// Registers a single object in the regstry
        /// </summary>
        /// <param name="type">the type of the object</param>
        /// <param name="identifier">the identifier that it will be known by</param>
        /// <param name="reference">a reference to the object</param>
        void Register ( Type type, string identifier, object reference );


        /// <summary>
        /// Return a reference currently stored in the registry.
        /// </summary>
        /// <typeparam name="T">The type of reference to return</typeparam>
        /// <param name="identifier">The identifier assigned to the reference</param>
        /// <returns>a reference to a generic object that matches both search criteria</returns>
        T LookUp<T>( string identifier );

        /// <summary>
        /// Count how many items are registered.
        /// </summary>
        int Count ();
    }
}