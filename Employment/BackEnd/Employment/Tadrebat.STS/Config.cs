using System;
using System.Collections.Generic;
using System.IO;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Employment.STS
{
    public class Config
    {
        //protected static IConfiguration _config;
        public static string urlstsAuthority = "";
        public static string urlSPAClient = "";

        public static string FacebookClientId = "";
        public static string FacebookSecret = "";
        public static string GoogleClientId = "";
        public static string GoogleSecret = "";
        public static string MicrosoftClientId = "";
        public static string MicrosoftAuthority = "";
        public static string TadrebatClientId = "";
        public static string TadrebatSecret = "";
        public static string TadrebatAuthority = "";
        public static string CertificatePath = "";
        public static string CertificatePassword = "";

        public static void SetupConfig()
        {
            IConfiguration _config = new ConfigurationBuilder()
               //.SetBasePath(Directory.GetCurrentDirectory())
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();

            //_config = objConfig;
            urlstsAuthority = _config.GetValue<string>("STSAuthorityURL");
            urlSPAClient = _config.GetValue<string>("SPAClientURL");

            FacebookClientId = _config.GetValue<string>("FacebookClientId");
            FacebookSecret = _config.GetValue<string>("FacebookSecret");
            GoogleClientId = _config.GetValue<string>("GoogleClientId");
            GoogleSecret = _config.GetValue<string>("GoogleSecret");
            MicrosoftClientId = _config.GetValue<string>("MicrosoftClientId");
            MicrosoftAuthority = _config.GetValue<string>("MicrosoftAuthority");
            TadrebatClientId = _config.GetValue<string>("TadrebatClientId");
            TadrebatSecret = _config.GetValue<string>("TadrebatSecret");
            TadrebatAuthority = _config.GetValue<string>("TadrebatAuthority");
            CertificatePath = _config.GetValue<string>("CertificatePath");
            CertificatePassword = _config.GetValue<string>("CertificatePassword");
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("projects-api", "Projects API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    //ClientId = "spa-codingofemployment-client",
                    ClientId = "87a10b87-XXXX-9457-083ed25faac5",
                    //ClientId = "87a10b87-5410-491b-9457-083ed25faac5", 
                    //ClientName = "Employment Projects SPA",
                    ClientName = "openid profile projects-api",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { urlSPAClient + "/signin-callback", urlSPAClient +  "/assets/silent-callback.html" },
                    PostLogoutRedirectUris = { urlSPAClient + "/signout-callback" },
                    AllowedCorsOrigins =     { urlSPAClient },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "projects-api"
                    },
                    AccessTokenLifetime = 10000
                }
                //,new Client
                //{
                //    ClientId = "mvc",
                //    ClientName = "MVC Client",
                //    AllowedGrantTypes = GrantTypes.Hybrid,

                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    RedirectUris           = { "http://localhost:4201/signin-oidc" },
                //    PostLogoutRedirectUris = { "http://localhost:4201/signout-callback-oidc" },

                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile
                //    },
                //    AllowOfflineAccess = true

                //}
            };

        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}