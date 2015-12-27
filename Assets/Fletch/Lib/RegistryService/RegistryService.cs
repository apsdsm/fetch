using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System;

namespace Fletch
{
    public class RegistryService : MonoBehaviour, IRegistryService
    {

        // private list of registrations
        private List<Registration> registrations;

        // private list of reservations
        private List<Reservation> reservations;


        /// <summary>
        /// Creates a new list to store registrations.
        /// </summary>
        void Awake ()
        {
            registrations = new List<Registration>();
            reservations = new List<Reservation>();
        }


        /// <summary>
        /// Get all registrations as an array.
        /// </summary>
        public Registration[] Registrations {
            get {
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

            // check to see if anybody wants this registration
            Reservation[] waitingList = reservations.Where( r => r.identifier == identifier && r.type == type ).ToArray();

            if ( waitingList.Length > 0 )
            {
                foreach ( Reservation reservation in waitingList )
                {
                    reservation.setter.Invoke( reservation.reserver, new object[] { reference } );
                    reservations.Remove( reservation );
                }
            }
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
        /// First checks to see if the object already exists in the list of registered components.
        /// If so, assigns that component to the reservation. If not, will create a new reservation
        /// containing the details that were passed to the method.
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="name">name the object will have</param>
        /// <param name="reservation">reference to variable that will be filled</param>
        public void Reserve<T>( string name, object from )
        {
            MethodInfo setterMethod = from.GetType().GetMethod( "set_" + name );

            if ( setterMethod == null )
            {
                throw new SetterNotFoundException( "no setter was found with the name " + name );
            }
            else
            {
                reservations.Add( new Reservation() { identifier = name, type = typeof( T ), reserver = from, setter = setterMethod } );
            }
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