using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityApi.Configuration;
using IdentityApi.IdentityServer;
using IdentityModel;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(IdentityApi.Startup))]

namespace IdentityApi
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
                    SiteName = "Fortegra IdentityServer",
                    SigningCertificate = X509.LocalMachine.My.SubjectDistinguishedName.Find(GlobalConfiguration.AuthorityCertificateSubject).First(),//CertificateLocator.Get(),
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
