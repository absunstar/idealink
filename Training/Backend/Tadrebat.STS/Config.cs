using System;
using System.Collections.Generic;
using System.IO;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Tadrebat.STS
{
    public class Config
    {
        //protected static IConfiguration _config;
        public static string urlstsAuthority = "";
        public static string urlSPAClient = "";
        public static string CertificatePath = "";
        public static string CertificatePassword = "";
        public static string urlEmploymentURL = "";

        public static void SetupConfig ()
        {
            IConfiguration _config = new ConfigurationBuilder()
               //.SetBasePath(Directory.GetCurrentDirectory())
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();

            //_config = objConfig;
            urlstsAuthority = _config.GetValue<string>("STSAuthorityURL");
            urlSPAClient = _config.GetValue<string>("SPAClientURL");
            CertificatePath = _config.GetValue<string>("CertificatePath");
            CertificatePassword = _config.GetValue<string>("CertificatePassword");
            urlEmploymentURL = _config.GetValue<string>("urlEmploymentURL");
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
                    ClientId = "spa-Tadrebat-client",
                    ClientName = "Tadrebat Projects SPA",
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
                    AccessTokenLifetime = 7200
                }
                ,new Client
                {
                    ClientId = "Tadrebat",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                     RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret("FKCbQSpycTlC_LCsIa0_rAAf")
                       // new Secret("secret".Sha256())
                    },

                    RedirectUris           = { Path.Combine(urlEmploymentURL, "signin-oidc-Tadrebat" )},
                   PostLogoutRedirectUris = { Path.Combine(urlEmploymentURL, "signout-callback-oidc-Tadrebat") },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                       // IdentityServerConstants.StandardScopes.Profile,
                       // "api"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,

                }
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