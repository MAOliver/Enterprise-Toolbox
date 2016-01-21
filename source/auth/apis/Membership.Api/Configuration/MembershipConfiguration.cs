using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityManager.Configuration;
using IdentityManager.Extensions;
using Membership.Api.IdentityManager;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.StaticFiles;

namespace Membership.Api.Configuration
{
    internal static class ConfigurationFactory
    {
        public static OpenIdConnectAuthenticationOptions CreateOpenIdConnectAuthenticationOptions()
        {
            return new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                Authority = GlobalConfiguration.AuthorityUri.ToString(),
                ClientId = "idmgr_client",
                RedirectUri = GlobalConfiguration.RedirectUri.ToString(),
                ResponseType = "id_token",
                UseTokenLifetime = false,
                Scope = "openid idmgr",
                SignInAsAuthenticationType = "Cookies",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = async n =>
                    {
                        if (n.ProtocolMessage.RequestType ==
                            Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                        {
                            var result = await n.OwinContext.Authentication.AuthenticateAsync("Cookies");
                            var idToken = result?.Identity.Claims.GetValue("id_token");
                            if (idToken != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idToken;
                                n.ProtocolMessage.PostLogoutRedirectUri = GlobalConfiguration.RedirectUri.ToString();
                            }
                        }
                    }
                }
            };
        }
        
        public static IdentityManagerOptions CreateIdentityManagerOptions()
        {
            var factory = new IdentityManagerServiceFactory();
            factory.ConfigureIdentityManagerService("IdentityManagerConfig");

            return new IdentityManagerOptions
            {
                Factory = factory,
                SecurityConfiguration = new HostSecurityConfiguration
                {
                    HostAuthenticationType = "Cookies"
                    , AdminRoleName = "IdentityManagerAdministrator"
                    , RequireSsl = GlobalConfiguration.RequireSSL ?? true
                }
            };
        }

        public static CookieAuthenticationOptions CreateCookieAuthenticationOptions()
        {
            return new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            };
        }
        
    }
}