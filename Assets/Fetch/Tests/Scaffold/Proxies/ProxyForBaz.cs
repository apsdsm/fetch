using UnityEngine;
using System.Collections.Generic;
using System;

namespace Fetch.Test
{
    public class ProxyForBaz : MonoBehaviour, IProxy<IBazService>
    {
        BazService baz;

        public IBazService GetProxy()
        {
            if (baz == null)
            {
                baz = new BazService();
            }

            return baz;
        }
    }
}
