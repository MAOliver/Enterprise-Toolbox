using System.Linq;
using System.Threading.Tasks;
using Auth.Api.IdentityServer;
using IdentityModel;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.WsFederation;
using Microsoft.Owin.StaticFiles;
using Owin;
using Serilog;

namespace Auth.Api.Configuration
{
    internal static class ConfigurationFactory
    {
        public static IdentityServerOptions CreateIdentityServerOptions()
        {
            return new IdentityServerOptions
            {
                SiteName = "IdentityServer3",
                SigningCertificate = X509.LocalMachine.My.SubjectDistinguishedName.Find("CN=sts").First(),
                RequireSsl = GlobalConfiguration.RequireSSL ?? true,
                PublicOrigin = GlobalConfiguration.PublicOrigin.ToString(),
                Factory = Factory.Configure("IdSvr3Config"),
                Endpoints = new EndpointOptions
                {
                    EnableCspReportEndpoint = true,
                },
                AuthenticationOptions = new AuthenticationOptions {EnablePostSignOutAutoRedirect = true, IdentityProviders = ConfigureAdditionalIdentityProviders},
                LoggingOptions = new LoggingOptions
                {
                    EnableKatanaLogging = true,
                    EnableHttpLogging = true,
                    EnableWebApiDiagnostics = true,
                    WebApiDiagnosticsIsVerbose = true
                },
                
                EventsOptions = new EventsOptions
                {
                    RaiseFailureEvents = true,
                    RaiseInformationEvents = true,
                    RaiseSuccessEvents = true,
                    RaiseErrorEvents = true
                }
            };   
        }

        public static void ConfigureAdditionalIdentityProviders(IAppBuilder app, string signInAsType)
        {
            var windowsAuthentication = new WsFederationAuthenticationOptions
            {
                AuthenticationType = "windows",
                Caption = "Windows",
                SignInAsAuthenticationType = signInAsType,
                MetadataAddress = GlobalConfiguration.WinAdMetadataUri.ToString(),
                Wtrealm = "urn:idsrv3",
                Wreply = $"{GlobalConfiguration.AuthorityRoute}/callback",
                Notifications = GetWsFedAuthNotifications()
            };
            app.UseWsFederationAuthentication(windowsAuthentication);
        }

        private static WsFederationAuthenticationNotifications GetWsFedAuthNotifications()
        {
            return new WsFederationAuthenticationNotifications
            {
                RedirectToIdentityProvider = notification =>
                {
                    if (notification.ProtocolMessage.IsSignOutMessage)
                    {
                        // tell IdentityServer to manage the sign out instead of the STS provider
                        notification.OwinContext.Authentication.SignOut();
                        notification.State = NotificationResultState.HandledResponse;
                    }
                    return Task.CompletedTask;
                }
            };
        }
        
    }
}