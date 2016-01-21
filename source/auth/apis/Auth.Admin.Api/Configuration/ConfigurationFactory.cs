using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Admin.Api.Config;
using Auth.Admin.Api.IdentityAdmin;
using IdentityAdmin.Configuration;
using IdentityAdmin.Extensions;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.StaticFiles;

namespace Auth.Admin.Api.Configuration
{
    internal static class ConfigurationFactory
    {
        public static OpenIdConnectAuthenticationOptions CreateOpenIdConnectAuthenticationOptions()
        {
            return new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                Authority = GlobalConfiguration.AuthorityUri.ToString(),
                ClientId = "idAdmin",
                RedirectUri = GlobalConfiguration.AuthAdminRedirectUri.ToString(),
                ResponseType = "id_token",
                UseTokenLifetime = false,
                Scope = "openid idAdmin",
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
                                n.ProtocolMessage.PostLogoutRedirectUri = GlobalConfiguration.AuthAdminRedirectUri.ToString();
                            }
                        }
                    }
                }
            };
        }

        public static IdentityAdminOptions CreateIdentityAdminOptions()
        {
            var factory = new IdentityAdminServiceFactory();
            
            factory.Configure();
            return new IdentityAdminOptions
            {
                Factory = factory,
                AdminSecurityConfiguration = new AdminHostSecurityConfiguration
                {
                    HostAuthenticationType = "Cookies",
                    AdminRoleName = "IdentityAdminAdministrator",
                    RequireSsl = GlobalConfiguration.RequireSSL ?? true
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