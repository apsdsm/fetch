using System;
using System.Reflection;

namespace Fletch
{
    /// <summary>
    /// Holds a single reservation that will be fulfilled in the future.
    /// </summary>
    public struct Reservation
    {
        // type of object reserved
        public Type type;

        // identifier for the reservation
        public string identifier;

        // object that made the reservation
        public object reserver;

        // setter that will be called when reservation can be fulfilled
        public MethodInfo setter;
    }
}
