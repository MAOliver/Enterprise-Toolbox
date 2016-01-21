using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Serilog;

namespace Auth.SelfHost
{
    

    public class Program
    {
        static void Main(string[] args)
        {
            var baseUri = ConfigurationManager.AppSettings["baseUri"];
            

            
            using (WebApp.Start<Startup>(baseUri))
            {
                Log.Logger = new LoggerConfiguration()
                   .WriteTo
                   //.LiterateConsole()
                   .LiterateConsole(outputTemplate: "{Timestamp:HH:MM} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}")
                   .MinimumLevel.Verbose()
                   .WriteTo
                   .RollingFile(pathFormat: @"c:\logs\IdSelfHost-{Date}.log")
                   .CreateLogger();

                Task.WhenAll
                (
                     Task.Run(async () => await TestConnection($"{baseUri}/ids/.well-known/openid-configuration"))
                     , Task.Run(async () => await TestConnection($"{baseUri}/idm"))
                     , Task.Run(async () => await TestConnection($"{baseUri}/idsadmin"))
                     , Task.Run(async () => await TestConnection($"{baseUri}/winad"))
                )
                .Wait(10000);
                Log.Information($"Server running at {baseUri} - press Enter to quit. ");
                Console.ReadLine();
            }    
        }

        static async Task TestConnection(string path)
        {
            var resultMessage = "";
            HttpStatusCode resultStatus;
            try
            {
                Log.Information($"testing path:{path}");
                var result = await new HttpClient().GetAsync(path);
                resultStatus = result.StatusCode;
                resultMessage = (await result.Content.ReadAsStringAsync()).Substring(0, 120) + "...";
            }
            catch (Exception ex)
            {
                resultMessage = $"message:{ex.Message},stack:{ex.ToString()}";
                resultStatus = HttpStatusCode.InternalServerError;
            }
            Log.Information($"path:{path},resultStatus:{resultStatus},resultMessage:{resultMessage}");
        }
    }
}
