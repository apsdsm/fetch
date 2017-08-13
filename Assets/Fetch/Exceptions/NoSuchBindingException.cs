using System;

namespace Fetch
{
    /// <summary>
    /// Called when the IOC cannot find a binding for the specified interface.
    /// </summary>
    public class NoSuchBindingException : Exception
    {
        public NoSuchBindingException() { }

        public NoSuchBindingException(string message) : base(message) { }

        public NoSuchBindingException(string message, Exception inner) : base(message, inner) { }
    }
}

