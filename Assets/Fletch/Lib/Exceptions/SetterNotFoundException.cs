using System;

namespace Fletch
{
    /// <summary>
    /// This exception is called when the RegistryService cannot find an appropriate setter for
    /// a reservation.
    /// </summary>
    public class SetterNotFoundException : Exception
    {
        public SetterNotFoundException () { }

        public SetterNotFoundException ( string message ) : base( message ) { }

        public SetterNotFoundException ( string message, Exception inner ) : base( message, inner ) { }
    }
}

