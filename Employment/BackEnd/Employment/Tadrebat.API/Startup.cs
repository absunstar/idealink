using System;
using System.Linq;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Employment.API.Helpers.Constants;
using SecuringAngularApps.API.Model;
using Microsoft.EntityFrameworkCore;
using Tadrebt.API.Data;
using Employment.ModelsGlobal;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Employment.Interface;
using Employment.Services;
using Employment.Cache;
using Employment.Notification;
using AutoMapper;
using Employment.Mongo.DataLayer;
using Employment.MongoDB.Interface;
using Newtonsoft.Json.Serialization;
using Employment.API.Helpers.AutoMapper;
using Employment.API.Helpers.HTTPCall;
using Employment.Persistance.Data;
using Employment.Persistance.Interfaces;

namespace Employment.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            //services.AddDbContext<ProjectDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("ProjectDbContext")));

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("STSConnection")));

            services.AddDbContext<EmploymentDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProjectDbContext")));

            services.AddSingleton<IMongoDBContext, MongoDBContext>(serviceProvider =>
            {
                var connectionString = Configuration.GetConnectionString("MongoConnection");
                var DBName = Configuration.GetValue<string>("MongoDBName");
                return new MongoDBContext(connectionString, DBName);
            });


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

            services.AddScoped<HelperMapperData>();
            services.AddScoped<HTTPCallSTS>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                    //builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => origin == "http://www.ms-employment.digisummits.com").AllowCredentials();
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                       .AddIdentityServerAuthentication(options =>
                       {
                           //options.Authority = "http://localhost:5500";
                           options.Authority = ConfigConstant.urlstsAuthority;
                           options.ApiName = "projects-api";
                           options.RequireHttpsMetadata = false;
                       });
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.EnableEndpointRouting = false;
            });

            //Need to add this line so Actions serialize sub object and return json
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllRequests");
            app.UseAuthentication();
            app.UseMvc();
        }
    }

}
