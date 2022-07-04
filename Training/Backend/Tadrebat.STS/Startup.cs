using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Tadrebat.Cache;
using Tadrebat.Interface;
using Tadrebat.ModelsGlobal;
using Tadrebat.Notification;
using Tadrebat.Services;
using Tadrebt.STS.Data;
using Tadrebt.STS.Quickstart.Account;

namespace Tadrebat.STS
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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped(typeof(IUserManagement), typeof(ServiceUserManagement));
            services.AddScoped(typeof(ICacheConfig), typeof(CacheConfig));
            services.AddScoped(typeof(INotificationEmail), typeof(NotificationEmail));


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => origin == Config.urlSPAClient).AllowCredentials();
                });
            });

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


            //SS for testing 
            IdentityModelEventSource.ShowPII = true;  //Addd for check logs 

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
                try
                {
                    builder.AddDeveloperSigningCredential();
                  
                }


                //     if (environment.isdevelopment())
                //{
                //    builder.adddevelopersigningcredential();
                //}
                //else
                //{
                //    try
                //    {
                //        var filename = config.certificatepath;

                //        if (!file.exists(filename))
                //        {
                //            throw new filenotfoundexception("no signing certificate!");
                //        }



                //        //var cert = new x509certificate2(filename, "password");
                //        var cert = new x509certificate2(filename, config.certificatepassword);

                //        builder.addsigningcredential(cert);
                //    }
                    catch (Exception ex)
                {
                    throw ex;
                }
               // var fileName = Path.Combine(@"C:\_NRG\", "EmploymentCertificate.pfx");

                //throw new Exception("need to configure key material");
            }
            //services.AddLocalization(options => options.ResourcesPath = "Resources");

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy", corsBuilder =>
            //    {
            //        corsBuilder.AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .SetIsOriginAllowed(origin => origin == Config.urlSPAClient)
            //        .AllowCredentials();
            //    });
            //});

            //services.AddMvc(options =>
            //{
            //    options.EnableEndpointRouting = false; //added this line when upgraded to .netcore 3.1
            //})
            //.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            //.AddDataAnnotationsLocalization();

            //services.AddTransient<IProfileService, CustomProfileService>();


            //var builder = services.AddIdentityServer(options =>
            //{
            //    options.Events.RaiseErrorEvents = true;
            //    options.Events.RaiseInformationEvents = true;
            //    options.Events.RaiseFailureEvents = true;
            //    options.Events.RaiseSuccessEvents = true;
            //    options.Authentication.CookieLifetime = TimeSpan.FromMinutes(15);
            //})
            //    .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //    .AddInMemoryApiResources(Config.GetApiResources())
            //    .AddInMemoryClients(Config.GetClients())
            //    .AddAspNetIdentity<ApplicationUser>()
            //    .AddProfileService<CustomProfileService>();


            //if (Environment.IsDevelopment())
            //{
            //    builder.AddDeveloperSigningCredential();
            //}
            //else
            //{
            //    //var fileName = Path.Combine(@"C:\_NRG\", "TadrebatCertificate.pfx");
            //    var fileName = Config.CertificatePath;

            //    if (!File.Exists(fileName))
            //    {
            //        throw new FileNotFoundException("No Signing Certificate!");
            //    }

            //    //var cert = new X509Certificate2(fileName, "password");
            //    var cert = new X509Certificate2(fileName, Config.CertificatePassword);

            //    builder.AddSigningCredential(cert);
            //    //throw new Exception("need to configure key material");
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCookiePolicy(); // Before UseAuthentication or anything else that writes cookies. 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            //var supportedCultures = new[]
            //{
            //    //https://www.infragistics.com/community/blogs/b/codefusion/posts/building-simple-multilingual-asp-net-core-website
            //    new CultureInfo("en-US"),
            //    new CultureInfo("ar-eg")
            //    //new CultureInfo("de-DE")
            //};
            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("en-US"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //});

            //app.UseStaticFiles();
            //app.UseCors("CorsPolicy");

            //app.UseIdentityServer();
            //app.UseMvcWithDefaultRoute();
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
