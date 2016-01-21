using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;

namespace Auth.Api.IdentityServer
{
    public static class CorsService
    {
        public static void RegisterCors(this IdentityServerServiceFactory factory, bool allowAll = false)
        {
            factory.CorsPolicyService = new Registration<ICorsPolicyService>(resolver => new DefaultCorsPolicyService { AllowAll = allowAll });
        }
    }
}