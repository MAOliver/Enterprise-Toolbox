using Identity.Core;
using Identity.Core.Services;
using IdentityServer3.Core.Configuration;
using IdentityServer3.EntityFramework;

namespace IdentityApi.IdentityServer
{
    internal class Factory
    {
        public static IdentityServerServiceFactory Configure(string connString)
        {
            var factory = new IdentityServerServiceFactory();

            factory.RegisterConfigurationServices(new EntityFrameworkServiceOptions
            {
                ConnectionString = connString,
                Schema = "clients"
            });
            factory.RegisterOperationalServices(new EntityFrameworkServiceOptions
            {
                ConnectionString = connString,
                Schema = "operations"
            });
            factory.RegisterCors(true);

            factory.ConfigureUserService(connString);

            return factory;
        }
    }
}