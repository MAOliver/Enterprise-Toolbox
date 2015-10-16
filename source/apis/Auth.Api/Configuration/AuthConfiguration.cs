using Auth.Api.IdentityServer;
using Identity.Core.Managers;
using IdentityServer3.Core.Configuration;

namespace Auth.Api.Configuration
{
    public static class AuthConfiguration
    {
        public static IdentityServerOptions CreateIdentityServerOptions()
        {
            return new IdentityServerOptions
            {
                SiteName = "IdentityServer",
                SigningCertificate = !GlobalConfiguration.IgnoreSsl.GetValueOrDefault()
                    ? new CertificateManager().GetCert(GlobalConfiguration.AuthorityCertificateThumbprint)
                    : new CertificateManager().GetTestCert(),//CertificateLocator.Get(),
                Factory = Factory.Configure("IdSvr3Config"),
                Endpoints = new EndpointOptions
                {
                    EnableCspReportEndpoint = true,
                },
                AuthenticationOptions = new AuthenticationOptions(),
                LoggingOptions = new LoggingOptions
                {
                    EnableKatanaLogging = true
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
    }
}