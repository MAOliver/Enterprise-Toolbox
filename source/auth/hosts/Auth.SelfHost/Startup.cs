using System.Web.Http;
using System.Web.Http.Dispatcher;
using Owin;

namespace Auth.SelfHost
{
    public class Startup
    {
        // This method is required by Katana:
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();
            
            //windows auth provider
            app.UseWindowsAuthentication();
            //oauth2+oidc source
            new WindowsAuthentication.Api.Startup().Configuration(app);
            new Auth.Api.Startup().Configuration(app);
            //user management api
            new Membership.Api.Startup().Configuration(app);
            //client+scopes management api
            new Auth.Admin.Api.Startup().Configuration(app);
            
            app.UseWebApi(webApiConfiguration);
        }


        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.Services.Replace(typeof(IAssembliesResolver), new ExternalAssembliesResolver());
            return config;
        }
    }
}