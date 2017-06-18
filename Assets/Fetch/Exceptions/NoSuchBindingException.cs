using System;

namespace Fetch
{
    /// <summary>
    /// This exception is called when the RegistryService cannot find an appropriate setter for
    /// a reservation.
    /// </summary>
    public class NoSuchBindingException : Exception
    {
        public NoSuchBindingException() { }

        public NoSuchBindingException(string message) : base(message) { }

        public NoSuchBindingException(string message, Exception inner) : base(message, inner) { }
    }
}

