using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Helpers.Constants
{
    public static class ConfigConstant
    {
        public static string urlstsAuthority = "";
        public static string urlSPAClient = "";
        public static string urlAPI = "";
        public static string urlCDN = "";
        public static int PageSize = 15;
        public static void SetupConfig()
        {
            IConfiguration config = new ConfigurationBuilder()
               //.SetBasePath(Directory.GetCurrentDirectory())
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();

            urlstsAuthority = config.GetValue<string>("STSAuthorityURL");
            urlSPAClient    = config.GetValue<string>("SPAClientURL");
            urlAPI = config.GetValue<string>("APIURL");
            urlCDN = config.GetValue<string>("FilesCDN");
            PageSize = config.GetValue<int>("PageSize");
        }
    }
}
