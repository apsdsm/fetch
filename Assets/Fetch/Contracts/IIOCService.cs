namespace Fetch
{
    public interface IIocService {
        
        /// <summary>
        /// Ask the IOC service to gather all children and make references to available
        /// services.
        /// </summary>
        void Populate ();

        /// <summary>
        /// Provides an array of available services.
        /// </summary>
        /// <returns></returns>
        Service[] Services { get; }
    }
}
