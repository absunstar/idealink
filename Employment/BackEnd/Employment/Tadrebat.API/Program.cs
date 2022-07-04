using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Employment.API.Helpers.Constants;

namespace Employment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigConstant.SetupConfig();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(ConfigConstant.urlAPI)
            .UseUrls("http://localhost:44323/")
            .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
