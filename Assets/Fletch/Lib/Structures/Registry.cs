using System;

namespace Fletch
{
    /// <summary>
    /// Holds a single object registration for any class that implements IRegistryService
    /// </summary>
    public struct Registration
    {
        public Type type;
        public string identifier;
        public object reference;
    }
}