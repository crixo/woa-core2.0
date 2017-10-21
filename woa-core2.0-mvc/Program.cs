using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Woa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
                .WriteTo.RollingFile("../Logs/woa-{Date}.txt")
            .CreateLogger();
            
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
               //.ConfigureAppConfiguration((hostingContext, config) =>
                   //{
                   //    var env = hostingContext.HostingEnvironment;
                   //    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   //          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                   //    config.AddEnvironmentVariables();
                   //})    
               //.ConfigureLogging((hostingContext, logging) =>
                    //{
                    //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    //    logging.AddConsole();
                    //    logging.AddDebug();
                    //})   
                .UseSerilog()
                .UseStartup<Startup>()
                .Build();
    }
}
