using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Web.Configuration;
using Membership.Api.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(Membership.Api.Startup))]

namespace Membership.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = Log.Logger ??
                new Serilog.LoggerConfiguration().MinimumLevel.Debug()
                    .WriteTo.RollingFile(pathFormat: @"c:\logs\IdMgr-{Date}.log")
                    .CreateLogger();

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseCookieAuthentication(ConfigurationFactory.CreateCookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(ConfigurationFactory.CreateOpenIdConnectAuthenticationOptions());
            app.Map(GlobalConfiguration.MembershipRoute, idm =>
            {
                idm.UseIdentityManager(ConfigurationFactory.CreateIdentityManagerOptions());
                idm.UseCors(CorsOptions.AllowAll);

            });
            
        }
    }
}
