﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CoreValidation.WorkloadTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            BuildWebHost(args, configuration).Run();
        }

        public static IWebHost BuildWebHost(string[] args, IConfiguration configuration)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .Build();
        }
    }
}