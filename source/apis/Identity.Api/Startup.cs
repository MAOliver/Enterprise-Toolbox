using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Auth.Api;
using Auth.Api.Configuration;
using Auth.Api.IdentityServer;
using Identity.Core.Managers;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(Startup))]

namespace Auth.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger =
                new Serilog.LoggerConfiguration().MinimumLevel.Debug()
                    .WriteTo.RollingFile(pathFormat: @"c:\logs\IdSvr-{Date}.log")
                    .CreateLogger();



            app.Map("/ids", idsrvApp =>
            {
                var idsOpts = new IdentityServerOptions
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
                    AuthenticationOptions = new AuthenticationOptions { },
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
                idsrvApp.UseIdentityServer(idsOpts);
            });
        }
    }

}
