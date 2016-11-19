using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fletch
{
    public interface IIocService
    {

        /// <summary>
        /// Ask the IOC service to gather all children and make references to available
        /// services.
        /// </summary>
        void Populate ();

        /// <summary>
        /// Provides an array of available services.
        /// </summary>
        /// <returns></returns>
        ServiceReference[] Services { get; }
    }
}
