using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(Auth.WebHost.Startup))]

namespace Auth.WebHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                   .WriteTo
                   .RollingFile(pathFormat: @"c:\logs\IdWebHost-{Date}.log")
                   .CreateLogger();

            new Auth.SelfHost.Startup().Configuration(app);
        }
        
    }


}
