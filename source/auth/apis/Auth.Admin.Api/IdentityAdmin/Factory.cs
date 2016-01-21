using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.EntityFramework;

namespace Auth.Admin.Api.IdentityAdmin
{
    internal class Factory
    {
        public static IdentityServerServiceFactory Configure(string connString)
        {
            var efOptions = new EntityFrameworkServiceOptions
            {
                ConnectionString = connString
            };
            var factory = new IdentityServerServiceFactory();
            //Clients are configured in auth.api
            //Scopes are configured in auth.api
            factory.RegisterConfigurationServices(efOptions);
            factory.RegisterOperationalServices(efOptions);
            factory.RegisterScopeStore(efOptions);
            factory.RegisterClientStore(efOptions);
            factory.RegisterCors(true);

            return factory;
        }
        
    }

    public static class CorsService
    {
        public static void RegisterCors(this IdentityServerServiceFactory factory, bool allowAll = false)
        {
            factory.CorsPolicyService = new Registration<ICorsPolicyService>(resolver => new DefaultCorsPolicyService { AllowAll = allowAll });
        }
    }
}