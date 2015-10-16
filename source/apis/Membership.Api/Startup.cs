using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Core;
using Identity.Core.Managers;
using Identity.Core.Stores;
using IdentityManager.Configuration;
using IdentityManager.Extensions;
using Membership.Api.Configuration;
using Membership.Api.IdentityManager;
using Microsoft.Owin;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(Membership.Api.Startup))]

namespace Membership.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger =
                new Serilog.LoggerConfiguration().MinimumLevel.Debug()
                    .WriteTo.RollingFile(pathFormat: @"c:\logs\IdMgr-{Date}.log")
                    .CreateLogger();


            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });
            
            app.UseOpenIdConnectAuthentication(new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                Authority = GlobalConfiguration.AuthorityUri.ToString(),
                ClientId = "idmgr_client",
                RedirectUri = GlobalConfiguration.RedirectUri.ToString(),
                ResponseType = "id_token",
                UseTokenLifetime = false,
                Scope = "openid idmgr",
                SignInAsAuthenticationType = "Cookies",
                Notifications = new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = async n =>
                    {
                        if (n.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                        {
                            var result = await n.OwinContext.Authentication.AuthenticateAsync("Cookies");
                            var idToken = result?.Identity.Claims.GetValue("id_token");
                            if (idToken != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idToken;
                                n.ProtocolMessage.PostLogoutRedirectUri = $"{ConfigurationManager.AppSettings["authUriBase"]}/idm";
                            }
                        }
                    }
                }
            });
            

            app.Map("/idm", idm =>
            {

                var factory = new IdentityManagerServiceFactory();
                factory.ConfigureIdentityManagerService("IdentityManagerConfig");

                idm.UseIdentityManager(new IdentityManagerOptions
                {
                    Factory = factory,
                    SecurityConfiguration = new HostSecurityConfiguration
                    {
                        HostAuthenticationType = "Cookies"
                        , AdminRoleName = "IdentityManagerAdministrator"
                    }
                });
            });
        }
    }
}
