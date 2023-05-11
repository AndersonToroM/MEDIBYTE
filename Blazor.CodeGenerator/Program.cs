using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CodeGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostsettings = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true)
              .Build();
            CreateWebHostBuilder(args, hostsettings).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfigurationRoot hostsettings) =>
           WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(hostsettings)
            .UseStartup<Startup>();
    }
}
