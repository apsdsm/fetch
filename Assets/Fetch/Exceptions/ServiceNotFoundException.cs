using System;

namespace Fetch
{
    /// <summary>
    /// Called when the IOC cannot resolve a requested service.
    /// </summary>
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException () { }

        public ServiceNotFoundException ( string message ) : base( message ) { }

        public ServiceNotFoundException ( string message, Exception inner ) : base( message, inner ) { }
    }
}

