using System.Configuration;
using Auth.Api;
using Auth.Api.Configuration;
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


            if (string.IsNullOrWhiteSpace(GlobalConfiguration.AuthorityRoute))
            {
                app.UseIdentityServer(AuthConfiguration.CreateIdentityServerOptions());
            }
            else
            {
                app.Map(GlobalConfiguration.AuthorityRoute, (idSvr) =>
                {
                    idSvr.UseIdentityServer(AuthConfiguration.CreateIdentityServerOptions());
                });
            }

            
            //alternatively can map to a url using app.Map...


        }

        
    }

}
