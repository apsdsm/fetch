namespace Fetch
{

    /// <summary>
    /// A Bridge is a link between a game obejct and some other non-monobehaviour
    /// object. By implementing the Bridge interface, the IOC knows to route requests
    /// for the non-monobehaviour class through the monobehaviour wrapper.
    /// </summary>
    /// <typeparam name="T">The type of object linked to by the bridge</typeparam>
    public interface IBridge<T> {

        /// <summary>
        /// Must provide a getter to the bridged object.
        /// </summary>
        T bridged { get; }

    }
}
