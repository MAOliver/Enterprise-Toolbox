using Identity.Core.Extensions;
using IdentityServer3.Core.Configuration;
using IdentityServer3.EntityFramework;

namespace Auth.Api.IdentityServer
{
    internal class Factory
    {
        public static IdentityServerServiceFactory Configure(string idsrvConnection)
        {
            var factory = new IdentityServerServiceFactory();

            factory.RegisterConfigurationServices(new EntityFrameworkServiceOptions
            {
                ConnectionString = idsrvConnection,
            });
            factory.RegisterOperationalServices(new EntityFrameworkServiceOptions
            {
                ConnectionString = idsrvConnection,
            });
            factory.RegisterScopeStore(new EntityFrameworkServiceOptions
            {
                ConnectionString = idsrvConnection
            });
            factory.RegisterCors(true);

            factory.ConfigureUserService(idsrvConnection);



            return factory;
        }
    }
}