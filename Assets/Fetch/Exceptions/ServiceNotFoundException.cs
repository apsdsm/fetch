using System;

namespace Fetch
{
    /// <summary>
    /// This exception is called when the RegistryService cannot find an appropriate setter for
    /// a reservation.
    /// </summary>
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException () { }

        public ServiceNotFoundException ( string message ) : base( message ) { }

        public ServiceNotFoundException ( string message, Exception inner ) : base( message, inner ) { }
    }
}

