using WindowsAuthentication.Api.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(WindowsAuthentication.Api.Startup))]

namespace WindowsAuthentication.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = Log.Logger ??
                new Serilog.LoggerConfiguration().MinimumLevel.Debug()
                    .WriteTo.RollingFile(pathFormat: @"c:\logs\WinAdSvr-{Date}.log")
                    .CreateLogger();

            app.Map(GlobalConfiguration.WinAdRoute, winad =>
            {
                winad.UseWindowsAuthenticationService(WinAdConfiguration.CreateWindowsAuthenticationOptions());
                winad.UseCors(CorsOptions.AllowAll);
            });
        }
    }
}
