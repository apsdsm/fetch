using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Fletch.Test
{
    /// <summary>
    /// This is a factory for building IOC containers. It's a fairly temporary
    /// class, and I indent on replacing it with something a little more generic
    /// in the future, but for now it stops me from writing the object creation
    /// code over and over and over.
    /// </summary>
    class IOCFactory
    {
        List<GameObject> instantiated;

        /// <summary>
        /// Set up a new list
        /// </summary>
        public IOCFactory ()
        {
            instantiated = new List<GameObject>();
        }

        /// <summary>
        /// Get rid of objects we instantiated.
        /// </summary>
        public void TearDown ()
        {
            foreach ( GameObject gameObject in instantiated )
            {
                if ( gameObject != null )
                {
                    GameObject.Destroy( gameObject );
                }
            }
        }
    
        /// <summary>
        /// Build an IOC Container with no services attached.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>reference to game object</returns>
        public GameObject WithNoServices ( )
        {
            GameObject ioc_object = new GameObject();
            ioc_object.AddComponent<IOC>();

            // add to instantiated list
            instantiated.Add( ioc_object );

            return ioc_object;
        }

        /// <summary>
        /// Build an IOC Container with a service attached
        /// </summary>
        /// <returns>reference to game object</returns>
        public GameObject WithService ( )
        {
            GameObject ioc_object = new GameObject();
            IOC ioc = ioc_object.AddComponent<IOC>();

            GameObject service_object = new GameObject();
            service_object.transform.parent = ioc_object.transform.parent;
            FletchTestService service = service_object.AddComponent<FletchTestService>();

            ioc.AddService( typeof( IFletchTestService ), service );

            instantiated.Add( ioc_object );
            instantiated.Add( service_object );

            return ioc_object;
        }
    }
}
