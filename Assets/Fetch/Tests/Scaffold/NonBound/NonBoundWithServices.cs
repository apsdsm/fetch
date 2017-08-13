namespace Fetch.Test
{
    public class NonBoundWithServices
    { 
        private IFooService fooService;

        public NonBoundWithServices(IFooService fooService) {
            this.fooService = fooService;
        }

        public IFooService GetFooService() {
            return this.fooService;
        }
    }
}
