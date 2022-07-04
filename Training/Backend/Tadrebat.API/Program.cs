using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tadrebat.API.Helpers.Constants;

namespace Tadrebat.API
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
               .UseUrls("https://localhost:44325/")
            .UseIIS()
                .UseStartup<Startup>()
                .Build();
    }
}
