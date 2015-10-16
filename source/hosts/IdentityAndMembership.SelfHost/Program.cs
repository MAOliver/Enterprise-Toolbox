using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin.Hosting;
using Owin;

namespace IdentityAndMembership.SelfHost
{
    

    public class Program
    {
        static void Main(string[] args)
        {
            var baseUri = "https://localhost:8086/";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);

           /* Task.WhenAll
            (
                Task.Run(async () => await TestConnection($"{baseUri}/ids"))
                , Task.Run(async () => await TestConnection($"{baseUri}/idm"))
            )
            .Wait(5000);*/

            Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);
            Console.ReadLine();
        }

        static async Task TestConnection(string path)
        {
            Console.WriteLine($"testing path:{path}");

            var result = await new HttpClient().GetAsync(path);
            Console.WriteLine($"path:{path},result:{result.StatusCode}");    
        }
    }

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

    public class ExternalAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var startupTypes = new List<Type>
            {
                typeof (Membership.Api.Startup),
                typeof (Auth.Api.Startup)
            };
            
            return base.GetAssemblies().Union(startupTypes.Select(Assembly.GetAssembly)).ToArray();
        }
    }
}
