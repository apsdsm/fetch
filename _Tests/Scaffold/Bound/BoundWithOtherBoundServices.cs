using System;

namespace Fetch.Test
{
    public class BoundWithOtherBoundServices : IBoundWithOtherBoundServices
    {
        private IBazService bazService;

        public BoundWithOtherBoundServices(IBazService bazService)
        {
            this.bazService = bazService;
        }

        public IBazService GetBazService()
        {
            return bazService;
        }
    }
}
