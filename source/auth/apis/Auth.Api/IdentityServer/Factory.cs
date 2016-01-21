using Identity.Core.Extensions;
using IdentityServer3.Core.Configuration;
using IdentityServer3.EntityFramework;

namespace Auth.Api.IdentityServer
{
    internal static class Factory
    {
        public static IdentityServerServiceFactory Configure(string idsrvConnection)
        {
            var factory = new IdentityServerServiceFactory();

            var efOptions = new EntityFrameworkServiceOptions
            {
                ConnectionString = idsrvConnection,
            };

            factory.RegisterConfigurationServices(efOptions);
            factory.RegisterOperationalServices(efOptions);
            factory.RegisterScopeStore(efOptions);
            factory.RegisterCors(true);

            factory.ConfigureUserService(idsrvConnection);



            return factory;
        }
    }
}