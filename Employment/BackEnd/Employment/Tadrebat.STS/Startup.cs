using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Employment.Cache;
using Employment.Interface;
using Employment.ModelsGlobal;
using Employment.Notification;
using Employment.Services;
using Tadrebt.STS.Data;
using Tadrebt.STS.Quickstart.Account;
using IdentityServer4;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.Options;
using Employment.MongoDB.Interface;
using Employment.Mongo.DataLayer;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SimpleCaptcha;
using Employment.Persistance.Interfaces;
using Employment.Persistance.Data;

namespace Employment.STS
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Config.SetupConfig();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("STSConnection")));

            services.AddDbContext<EmploymentDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("ProjectDbContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IMongoDBContext, MongoDBContext>(serviceProvider =>
            {
                var connectionString = Configuration.GetConnectionString("MongoConnection");
                var DBName = Configuration.GetValue<string>("MongoDBName");
                return new MongoDBContext(connectionString, DBName);
            });

            //services.AddScoped(typeof(IUserManagement), typeof(ServiceUserManagement));
            //services.AddScoped(typeof(ICacheConfig), typeof(CacheConfig));
            //services.AddScoped(typeof(INotificationEmail), typeof(NotificationEmail));

            services.AddScoped(typeof(ICacheConfig), typeof(CacheConfig));
            services.AddScoped(typeof(INotificationEmail), typeof(NotificationEmail));

            services.AddScoped(typeof(IUserManagement), typeof(ServiceUserManagement));
            services.AddScoped(typeof(IDataManagement), typeof(ServiceDataManagement));
            services.AddScoped(typeof(IFile), typeof(ServiceFile));

            services.AddScoped(typeof(IServiceRepository<>), typeof(ServiceRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EmploymentRepository<>));
            services.AddScoped(typeof(IUserProfile), typeof(ServiceUserProfile));
            services.AddScoped(typeof(IServiceLanguages), typeof(ServiceLanguage));
            services.AddScoped(typeof(IServiceCountry), typeof(ServiceCountry));
            services.AddScoped(typeof(IServiceIndustry), typeof(ServiceIndustry));
            services.AddScoped(typeof(IServiceJobFields), typeof(ServiceJobFields));
            services.AddScoped(typeof(IServiceQualification), typeof(ServiceQualification));
            services.AddScoped(typeof(IServiceYearsOfExperience), typeof(ServiceYearsOfExperience));
            services.AddScoped(typeof(IServiceCompany), typeof(ServiceCompany));
            services.AddScoped(typeof(IServiceJob), typeof(ServiceJob));
            services.AddScoped(typeof(IServiceJobSeeker), typeof(ServiceJobSeeker));
            services.AddScoped(typeof(IServiceFavourite), typeof(ServiceFavourite));
            services.AddScoped(typeof(IServiceApply), typeof(ServiceApply));
            services.AddScoped(typeof(IServiceJobFair), typeof(ServiceJobFair));
            services.AddScoped(typeof(IServiceConfigForm), typeof(ServiceConfigForm));

            services.AddScoped(typeof(IDBNGOType), typeof(DBNGOType));
            services.AddScoped(typeof(IDBUserProfile), typeof(DBUserProfile));
            services.AddScoped(typeof(IDBLanguages), typeof(DBLanguages));
            services.AddScoped(typeof(IDBCountry), typeof(DBCountry));
            services.AddScoped(typeof(IDBJobFields), typeof(DBJobFields));
            services.AddScoped(typeof(IDBQualification), typeof(DBQualification));
            services.AddScoped(typeof(IDBIndustry), typeof(DBIndustry));
            services.AddScoped(typeof(IDBYearsOfExperience), typeof(DBYearsOfExperience));
            services.AddScoped(typeof(IDBCompany), typeof(DBCompany));
            services.AddScoped(typeof(IDBJob), typeof(DBJob));
            services.AddScoped(typeof(IDBJobSeeker), typeof(DBJobSeeker));
            services.AddScoped(typeof(IDBFavourite), typeof(DBFavourite));
            services.AddScoped(typeof(IDBApply), typeof(DBApply));
            services.AddScoped(typeof(IDBJobFair), typeof(DBJobFair));
            services.AddScoped(typeof(IDBConfigForm), typeof(DBConfigForm));

            //services.AddScoped<HelperMapperData>();
            //services.AddScoped<HTTPCallSTS>();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder =>
                {
                    //corsBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    corsBuilder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => origin == Config.urlSPAClient).AllowCredentials();
                });
            });
            services.AddSession();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false; //added this line when upgraded to .netcore 3.1
            });
            services.AddTransient<IProfileService, CustomProfileService>();


            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Authentication.CookieLifetime = TimeSpan.FromMinutes(15);
            })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<CustomProfileService>();


            if (!string.IsNullOrEmpty(Config.GoogleClientId) && !string.IsNullOrEmpty(Config.GoogleSecret))
            {
                services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = Config.GoogleClientId;
                    options.ClientSecret = Config.GoogleSecret;
                    options.Scope.Clear();
                    options.Scope.Add("openid https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile");

                });
            }
            if (!string.IsNullOrEmpty(Config.FacebookClientId) && !string.IsNullOrEmpty(Config.FacebookSecret))
            {
                services.AddAuthentication().AddFacebook("Facebook", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                options.ClientId = Config.FacebookClientId;
                options.ClientSecret = Config.FacebookSecret;
                options.Scope.Clear();
                options.Scope.Add("openid email");

            });
            }
            if (!string.IsNullOrEmpty(Config.MicrosoftAuthority) && !string.IsNullOrEmpty(Config.MicrosoftClientId))
            {
                //https://github.com/Crokus/aspnetcore-idsrv-aad-example
                services.AddAuthentication().AddOpenIdConnect("AAD", "Azure", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.Authority = Config.MicrosoftAuthority;
                    options.ClientId = Config.MicrosoftClientId;
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.CallbackPath = "/signin-oidc";

                    //options.Scope.Add("profile");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false
                    };
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ResponseType = "code";


                });
            }
            if (!string.IsNullOrEmpty(Config.TadrebatAuthority) && !string.IsNullOrEmpty(Config.TadrebatClientId) && !string.IsNullOrEmpty(Config.TadrebatSecret))
            {
                services.AddAuthentication().AddOpenIdConnect("Tadrebat", "Tadrebat", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.CallbackPath = @"/signin-oidc-Tadrebat";
                    options.SignedOutCallbackPath = @"/signout-callback-oidc-Tadrebat";
                    options.SignedOutRedirectUri = Config.urlSPAClient;
                    options.Authority = Config.TadrebatAuthority;
                    options.ClientId = Config.TadrebatClientId;
                    options.ClientSecret = Config.TadrebatSecret;
                    options.RequireHttpsMetadata = false;
                    options.Scope.Clear();
                    options.Scope.Add("openid");


                });
            }

            //for error: chrome Exception: Correlation failed. Unknown location            
            //https://community.auth0.com/t/correlation-failed-unknown-location-error-on-chrome-but-not-in-safari/40013/6
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                //if (string.IsNullOrEmpty(Config.CertificatePath) || string.IsNullOrEmpty(Config.CertificatePassword))
                //{
                //    throw new Exception("certificate not setup");
                //}
                
                //var fileName = Path.Combine(@"C:\_NRG\", "EmploymentCertificate.pfx");
                var fileName = Config.CertificatePath;
                //var fileName = Environment.ContentRootPath + Config.CertificatePath;
                //if (!File.Exists(fileName))
                //{
                //    throw new FileNotFoundException("No Signing Certificate!");
                //}

                //var cert = new X509Certificate2(fileName, "password");
                var cert = new X509Certificate2(fileName, Config.CertificatePassword);

                builder.AddSigningCredential(cert);
                //throw new Exception("need to configure key material");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCookiePolicy(); // Before UseAuthentication or anything else that writes cookies. 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseIdentityServer(); // wel-known/config

            // configure Session middleware
            app.UseSession();

            //https://github.com/art264400/SimpleCaptcha
            // configure your application pipeline to use Captcha middleware
            // Important! UseCaptcha(...) must be called after the UseSession() call
            app.UseCaptcha(Configuration);

            app.UseMvcWithDefaultRoute();
        }

        //for error: chrome Exception: Correlation failed. Unknown location
        //https://community.auth0.com/t/correlation-failed-unknown-location-error-on-chrome-but-not-in-safari/40013/6
        private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }

        //for error: chrome Exception: Correlation failed. Unknown location
        //https://community.auth0.com/t/correlation-failed-unknown-location-error-on-chrome-but-not-in-safari/40013/6
        public bool DisallowsSameSiteNone(string userAgent)
        {
            // Check if a null or empty string has been passed in, since this
            // will cause further interrogation of the useragent to fail.
            if (String.IsNullOrWhiteSpace(userAgent))
                return false;

            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking
            // stack.
            if (userAgent.Contains("CPU iPhone OS 12") ||
                userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack. 
            // This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            //if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            if (userAgent.Contains("Chrome"))
            {
                return true;
            }

            return false;
        }
    }
}
