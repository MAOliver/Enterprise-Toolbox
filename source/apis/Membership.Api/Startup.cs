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
            app.UseCookieAuthentication(AuthConfiguration.CreateCookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(AuthConfiguration.CreateOpenIdConnectAuthenticationOptions());
            
            if (string.IsNullOrWhiteSpace(GlobalConfiguration.MembershipRoute))
            {
                app.UseIdentityManager(AuthConfiguration.CreateIdentityManagerOptions());
            }
            else
            {
                app.Map(GlobalConfiguration.MembershipRoute, idm =>
                {
                    idm.UseIdentityManager(AuthConfiguration.CreateIdentityManagerOptions());
                });
            }


        }
    }
}
