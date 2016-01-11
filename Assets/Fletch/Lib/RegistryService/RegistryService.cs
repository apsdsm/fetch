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
        private List<Registration> registrations = new List<Registration>();

        // private list of reservations
        private List<Reservation> reservations = new List<Reservation>();

        /// <summary>
        /// Get all registrations as an array.
        /// </summary>
        public Registration[] Registrations {
            get {
                return registrations.ToArray();
            }
        }


        /// <summary>
        /// Register a new object using generics.
        /// </summary>
        /// <typeparam name="T">type of object to register as</typeparam>
        /// <param name="identifier">string identifier for object</param>
        /// <param name="reference">reference to object</param>
        public void Register<T>( string identifier, object reference )
        {
            Registration registration = new Registration();

            registration.type = typeof( T );
            registration.identifier = identifier;
            registration.reference = reference;

            registrations.Add( registration );

            // check to see if anybody wants this registration
            Reservation[] waitingList = reservations.Where( r => r.identifier == identifier && r.type == typeof( T ) ).ToArray();

            if ( waitingList.Length > 0 )
            {
                foreach ( Reservation reservation in waitingList )
                {
                    SendReferenceToReserver( reservation.setter, reservation.reserver, reference );
                    reservations.Remove( reservation );
                }
            }
        }

        /// <summary>
        /// Deletes any existing registrations and reservations.
        /// </summary>
        public void Flush ()
        {
            registrations.Clear();
            reservations.Clear();
        }

        /// <summary>
        /// Will send the specified reference to the object that made the reservation.
        /// </summary>
        /// <param name="setter">setter method to use</param>
        /// <param name="reserver">the object to send the method invocation to</param>
        /// <param name="reference">the object reference that will be sent in the invocation</param>
        private void SendReferenceToReserver ( MethodInfo setter, object target, object reference )
        {
            setter.Invoke( target, new object[] { reference } );
        }

        /// <summary>
        /// Deregisters an existing object from the registry.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        public void Deregister<T>( string identifier )
        {
            object reference = registrations.RemoveAll( r => r.type == typeof ( T ) && r.identifier == identifier );
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

            T lookUp = LookUp<T>( name );

            if ( lookUp != null )
            {
                SendReferenceToReserver( setterMethod, from, lookUp );
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