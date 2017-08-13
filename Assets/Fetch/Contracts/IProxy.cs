namespace Fetch
{
    /// <summary>
    /// IProxy allows you to proxy a reference to a service via a monobehaviour wrapper.
    /// </summary>
    public interface IProxy<T>
    {
        /// <summary>
        /// This method must provide an instance of the proxied class.
        /// </summary>
        /// <returns>Reference to instance of T</returns>
        T GetProxy();
    }
}
