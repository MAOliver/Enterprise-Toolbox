using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;
using Auth.Admin.Api.Config;
using Auth.Admin.Api.Configuration;
using IdentityAdmin.Configuration;
using IdentityAdmin.Extensions;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.StaticFiles;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(Auth.Admin.Api.Startup))]

namespace Auth.Admin.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = Log.Logger ??
                 new Serilog.LoggerConfiguration().MinimumLevel.Debug()
                     .WriteTo.RollingFile(pathFormat: @"c:\logs\IdSvrAdmin-{Date}.log")
                     .CreateLogger();

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseCookieAuthentication(ConfigurationFactory.CreateCookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(ConfigurationFactory.CreateOpenIdConnectAuthenticationOptions());

            app.Map(GlobalConfiguration.AuthAdminRoute, adminApp =>
            {
                adminApp.UseIdentityAdmin(ConfigurationFactory.CreateIdentityAdminOptions());
                adminApp.UseCors(CorsOptions.AllowAll);

            });
        }
    }
    
}
