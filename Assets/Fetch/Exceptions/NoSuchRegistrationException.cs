using System;

namespace Fetch
{
    /// <summary>
    /// Called when the IOC cannot find a registration matching a user query.
    /// </summary>
    public class NoSuchRegistrationException : Exception
    {
        public NoSuchRegistrationException() { }

        public NoSuchRegistrationException(string message) : base(message) { }

        public NoSuchRegistrationException(string message, Exception inner) : base(message, inner) { }
    }
}

