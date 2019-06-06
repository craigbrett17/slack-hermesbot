using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static System.Console;

namespace HermesBot.Service
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            WriteLine("Launching service... Loading configuration");

            var host = new HostBuilder()
                .UseConsoleLifetime()
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.SetBasePath(Directory.GetCurrentDirectory());
                    hostConfig.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration(ConfigureSettingsProviders)
                .ConfigureServices(ConfigureServices)
                .Build();

            await host.RunAsync();
            WriteLine("Terminating application. Bye!");
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var travelStreams = hostContext.Configuration.GetSection("TravelStreams").Get<Dictionary<string, Config.TravelStream>>();
            if (travelStreams.ContainsKey("TFL") && travelStreams["TFL"].Enabled)
            {

            }
        }

        private static void ConfigureSettingsProviders(HostBuilderContext hostContext, IConfigurationBuilder configBuilder)
        {
            configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configBuilder.AddJsonFile("travelstreams.json", optional: false, reloadOnChange: true);
            configBuilder.AddEnvironmentVariables();

            var devEnvironmentVariable = Environment.GetEnvironmentVariable("ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";

            if (isDevelopment)
            {
                configBuilder.AddUserSecrets<Config.Secrets>();
            }
        }
    }
}
