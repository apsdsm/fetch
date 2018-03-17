using UnityEngine;
using System.Collections.Generic;

namespace Fetch.Test
{
    public class DummyServiceProvider : ServiceProvider
    {
        void Populate()
        {
            // bind services
            Bind<IBound, Bound>();
            Bind<IBoundWithServices, BoundWithServices>();
            Bind<IBoundWithParams, BoundWithParams>();
            Bind<IBoundWithSharedTypeParams, BoundWithSharedTypeParams>();
            Bind<IBoundWithServicesAndParams, BoundWithServicesAndParams>();
            Bind<IBoundWithOtherBoundServices, BoundWithOtherBoundServices>();

            // set service as singleton
            Singleton<IBazService, BazService>();
        }
    }
}
