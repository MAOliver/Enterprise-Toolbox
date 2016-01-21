using System.Configuration;
using Auth.Api;
using Auth.Api.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(Startup))]

namespace Auth.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = Log.Logger ??
                 new Serilog.LoggerConfiguration().MinimumLevel.Debug()
                     .WriteTo.RollingFile(pathFormat: @"c:\logs\IdSvr-{Date}.log")
                     .CreateLogger();

            app.Map(GlobalConfiguration.AuthorityRoute, (idSvr) =>
            {
                idSvr.UseIdentityServer(ConfigurationFactory.CreateIdentityServerOptions());
                idSvr.UseCors(CorsOptions.AllowAll);
            });
        }

        
    }

}
