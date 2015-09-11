using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityManager.Configuration;
using IdentityManager.Extensions;
using Membership.Api.Configuration;
using Membership.Api.IdentityManager;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Membership.Api.Startup))]

namespace Membership.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
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
                            if (result != null)
                            {
                                var id_token = result.Identity.Claims.GetValue("id_token");
                                if (id_token != null)
                                {
                                    n.ProtocolMessage.IdTokenHint = id_token;
                                    n.ProtocolMessage.PostLogoutRedirectUri = $"{ConfigurationManager.AppSettings["authUriBase"]}/idm";
                                }
                            }
                        }
                    }
                }
            });

            app.Map("/idm", idm =>
            {

                var factory = new IdentityManagerServiceFactory();
                factory.ConfigureIdentityManagerService("IdSvr3Config");

                idm.UseIdentityManager(new IdentityManagerOptions
                {
                    Factory = factory,
                    SecurityConfiguration = new HostSecurityConfiguration
                    {
                        HostAuthenticationType = "Cookies"
                    }
                });
            });
        }
    }
}
