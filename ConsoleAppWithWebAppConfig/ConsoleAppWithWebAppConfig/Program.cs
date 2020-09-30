using System;
using System.IO;
using ConsoleAppWithWebAppConfig.Abstraction;
using ConsoleAppWithWebAppConfig.Implementation;
using ConsoleAppWithWebAppConfig.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConsoleAppWithWebAppConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Started..");
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IGreetingService, GreetingService>();
                    services.AddScoped<IProcess, Process>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);
            svc.Run();

            var application = ActivatorUtilities.GetServiceOrCreateInstance<IProcess>(host.Services);
            application.ProcessStarter();

        }


        // Set Up logging..
        static void BuildConfig(IConfigurationBuilder builder)
        {
            // current directory where you are running, look for app-settings.Json. and thats not optional..
            // Get app-settings dev / production.Json... what every env you are running on

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    optional: true)
                .AddEnvironmentVariables();
        }
    }
}
