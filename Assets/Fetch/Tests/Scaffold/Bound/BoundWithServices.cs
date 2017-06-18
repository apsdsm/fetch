namespace Fetch.Test
{
    public class BoundWithServices : IBoundWithServices
    { 
        private IFooService fooService;

        public BoundWithServices(IFooService fooService) {
            this.fooService = fooService;
        }

        public IFooService GetFooService() {
            return this.fooService;
        }
    }
}
