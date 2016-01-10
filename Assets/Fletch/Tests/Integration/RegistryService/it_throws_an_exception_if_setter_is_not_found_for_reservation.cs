using UnityEngine;
using Flexo;

namespace Fletch.Test.Integration.RegistryServiceTests
{

    [IntegrationTest.DynamicTest( "Fletch.RegistryService" )]
    public class it_throws_an_exception_if_setter_is_not_found_for_reservation : registry_service_test
    {
        void Test ()
        {
            try
            {
                registry.Reserve<BazComponent>( "NotValid", this );

            }
            catch ( SetterNotFoundException e )
            {
                IntegrationTest.Assert( e.Message == "no setter was found with the name NotValid", "wrong message in exception: " + e.Message );
                IntegrationTest.Pass();
            }
            finally
            {
                registry.Flush();
            }
        }
    }
}