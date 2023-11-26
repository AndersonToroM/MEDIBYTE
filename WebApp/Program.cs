using Dominus.Backend.Application;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Blazor.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("es");
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            if (!Directory.Exists(DApp.PathDirectoryLogs))
                Directory.CreateDirectory(DApp.PathDirectoryLogs);

            var assembly = Assembly.GetExecutingAssembly().GetName();

            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", $"{assembly.Name}")
            //.WriteTo.Console()
            .Filter.ByExcluding(c => c.MessageTemplate.Text.Contains("Authorization was successful"))
            .WriteTo.File(Path.Combine(DApp.PathDirectoryLogs, "log.log"),
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
            var hostsettings = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", true)
             .Build();

            CreateHostBuilder(args, hostsettings).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot hostsettings) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog().UseConfiguration(hostsettings)
                .UseStartup<Startup>();

        //public static string DirectoryLog = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Files", "Log");

        //public static void Main(string[] args)
        //{
        //    CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("es");
        //    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        //    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

        //    var hostsettings = new ConfigurationBuilder()
        //       .SetBasePath(Directory.GetCurrentDirectory())
        //       .AddJsonFile("appsettings.json", true)
        //       .Build();
        //    CreateWebHostBuilder(args, hostsettings).Build().Run();
        //}

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfigurationRoot hostsettings) =>
        //  WebHost.CreateDefaultBuilder(args)
        //    .UseConfiguration(hostsettings)
        //    .UseStartup<Startup>();
    }
}
