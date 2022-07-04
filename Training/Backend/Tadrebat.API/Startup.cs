using System;
using System.Linq;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tadrebat.API.Helpers.Constants;
using SecuringAngularApps.API.Model;
using Microsoft.EntityFrameworkCore;
using Tadrebt.API.Data;
using Tadrebat.ModelsGlobal;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Tadrebat.Interface;
using Tadrebat.Services;
using Tadrebat.Cache;
using Tadrebat.Notification;
using Tadrebat.Persistance.Interfaces;
using Tadrebat.Persistance.Data;
using AutoMapper;
using Tadrebat.Mongo.DataLayer;
using Tadrebat.MongoDB.Interface;
using Newtonsoft.Json.Serialization;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Helpers.HTTPCall;
using Microsoft.AspNetCore.Http.Features;
using SQL;
using Microsoft.IdentityModel.Logging;

namespace Tadrebat.API
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

            services.AddDbContext<ProjectDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProjectDbContext")));

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("STSConnection")));

            services.AddDbContext<TadrebatDbContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("ProjectDbContext")));

            services.AddSingleton<IMongoDBContext, MongoDBContext>(serviceProvider =>
            {
                var connectionString = Configuration.GetConnectionString("MongoConnection");
                var DBName = Configuration.GetValue<string>("MongoDBName");
                return new MongoDBContext(connectionString, DBName);
            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(TadrebatRepository<>));
            services.AddScoped(typeof(IServiceRepository<>), typeof(ServiceRepository<>));

            services.AddScoped(typeof(ICacheConfig), typeof(CacheConfig));
            services.AddScoped(typeof(INotificationEmail), typeof(NotificationEmail));

            services.AddScoped(typeof(IUserManagement), typeof(ServiceUserManagement));
            services.AddScoped(typeof(IDataManagement), typeof(ServiceDataManagement));
            //services.AddScoped(typeof(IEntityManagement), typeof(ServiceEntityManagement));

            services.AddScoped<ServiceEntityManagement>();
            services.AddScoped<ServiceSQL>();
            services.AddScoped<ServiceUpdateEntityConsistency>();

            services.AddScoped(typeof(IUserProfile), typeof(ServiceUserProfile));
            services.AddScoped(typeof(ITrainee), typeof(ServiceTrainee));
            services.AddScoped(typeof(ITraining), typeof(ServiceTraining));
            services.AddScoped(typeof(IQuestion), typeof(ServiceQuestion));
            services.AddScoped(typeof(IExam), typeof(ServiceExam));
            services.AddScoped(typeof(IServiceConfigForm), typeof(ServiceConfigForm));
            services.AddScoped(typeof(IContentData), typeof(ServiceContentData));
            services.AddScoped(typeof(ICertificate), typeof(ServiceCertificate));
            services.AddScoped(typeof(IExamTemplate), typeof(ServiceExamTemplate));
            services.AddScoped(typeof(ILogoPartner), typeof(ServiceLogoPartner));

            services.AddScoped(typeof(IDBTrainingCategory), typeof(DBTrainingCategory));
            services.AddScoped(typeof(IDBCity), typeof(DBCity));
            services.AddScoped(typeof(IDBNGOType), typeof(DBNGOType));
            services.AddScoped(typeof(IDBTrainingType), typeof(DBTrainingType));
            services.AddScoped(typeof(IDBEntityPartner), typeof(DBEntityPartner));
            services.AddScoped(typeof(IDBEntitySubPartner), typeof(DBEntitySubPartner));
            services.AddScoped(typeof(IDBEntityTrainingCenter), typeof(DBEntityTrainingCenter));
            services.AddScoped(typeof(IDBUserProfile), typeof(DBUserProfile));
            services.AddScoped(typeof(IDBTrainee), typeof(DBTrainee));
            services.AddScoped(typeof(IDBTraining), typeof(DBTraining));
            services.AddScoped(typeof(IDBQuestion), typeof(DBQuestion));
            services.AddScoped(typeof(IDBExam), typeof(DBExam));
            services.AddScoped(typeof(IDBConfigForm), typeof(DBConfigForm));
            services.AddScoped(typeof(IDBContentData), typeof(DBContentData));
            services.AddScoped(typeof(IDBCertificate), typeof(DBCertificate));
            services.AddScoped(typeof(IDBExamTemplate), typeof(DBExamTemplate));
            services.AddScoped(typeof(IDBLogoPartner), typeof(DBLogoPartner));

            services.AddScoped<HelperMapperCertificate>();
            services.AddScoped<HelperMapperQuestion>();
            services.AddScoped<HelperMapperEntity>();
            services.AddScoped<HelperMapperData>();
            services.AddScoped<HelperMapperUser>();
            services.AddScoped<HelperMapperTraining>();
            services.AddScoped<HelperTranslate>();
            services.AddScoped<HTTPCallSTS>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                   // builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => origin == "http://www.ms-training.digisummits.com").AllowCredentials();
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                       .AddIdentityServerAuthentication(options =>
                       {
                           //options.Authority = "http://sts.ms-training.digisummits.com";
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

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120);
            });

            //Need to add this line so Actions serialize sub object and return json
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            //To avoid the MultiPartBodyLength error, we are going to modify our configuration in the Startup.cs class:
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            //SS for testing 
            IdentityModelEventSource.ShowPII = true;  //Addd for check logs 
        }

       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllRequests");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseSession();

            app.UseAuthorization(); //For testing
            app.UseAuthentication();
            app.UseMvc();
        }
    }

}
