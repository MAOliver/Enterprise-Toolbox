using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Serilog;

namespace IdentityAndMembership.SelfHost
{
    

    public class Program
    {
        static void Main(string[] args)
        {
            var baseUri = ConfigurationManager.AppSettings["baseUri"];
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .LiterateConsole(outputTemplate: "{Timestamp:HH:MM} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}")
                .CreateLogger();

            using (WebApp.Start<Startup>(baseUri))
            {
                Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);
                Task.WhenAll
                (
                     Task.Run(async () => await TestConnection($"{baseUri}/ids"))
                     , Task.Run(async () => await TestConnection($"{baseUri}/idm"))
                )
                .Wait(5000);
                Console.ReadLine();
            }    
        }

        static async Task TestConnection(string path)
        {
            var resultMessage = "";
            var resultStatus = HttpStatusCode.OK;
            try
            {
                Console.WriteLine($"testing path:{path}");
                var result = await new HttpClient().GetAsync(path);
                resultStatus = result.StatusCode;
                resultMessage = "a bunch of html";
            }
            catch (Exception ex)
            {
                resultMessage = $"message:{ex.Message},stack:{ex.ToString()}";
                resultStatus = HttpStatusCode.InternalServerError;
            }
            Console.WriteLine($"path:{path},resultStatus:{resultStatus},resultMessage:{resultMessage}");

        }
    }
}
