using System.Web.Http;
using System.Web.Http.Dispatcher;
using Owin;

namespace IdentityAndMembership.SelfHost
{
    public class Startup
    {
        // This method is required by Katana:
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();
            new Auth.Api.Startup().Configuration(app);
            new Membership.Api.Startup().Configuration(app);

            // Use the extension method provided by the WebApi.Owin library:
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