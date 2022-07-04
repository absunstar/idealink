using Employment.Cache;
using Employment.Interface;
using Employment.Mongo.DataLayer;
using Employment.MongoDB.Interface;
using Employment.Notification;
using Employment.Persistance.Data;
using Employment.Persistance.Interfaces;
using Employment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UpdateExpiredJobs
{
    class Program
    {
        public IConfiguration Configuration { get; }
        private static IServiceProvider _serviceProvider;
        public Program(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        public static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Updating Expired Jobs");
            
            try
            {
                RegisterServices();

                var _BLService = _serviceProvider.GetService<IServiceJob>();
                await _BLService.UpdateExpiredJobs();


            }
            catch (Exception ex)
            {
                var rootFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogErrors");
                var fileName = DateTime.Now.ToString("yyyyMMdd HH-mm-ss fff") + ".txt";
                var savePath = Path.Combine(rootFolder, fileName);

                if (!Directory.Exists(rootFolder))
                    Directory.CreateDirectory(rootFolder);

                using (StreamWriter file = System.IO.File.CreateText(savePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, ex.Message + ex.InnerException);
                }
            }
            DisposeServices();
        }
        private static void RegisterServices()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var collection = new ServiceCollection();
            collection.AddSingleton<IConfiguration>(Configuration);
            collection.AddSingleton<IMongoDBContext, MongoDBContext>(serviceProvider =>
            {
                var connectionString = Configuration.GetConnectionString("MongoConnection");
                var DBName = Configuration.GetValue<string>("MongoDBName");
                return new MongoDBContext(connectionString, DBName);
            });
            collection.AddDbContext<EmploymentDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("ProjectDbContext")));


            collection.AddMemoryCache();
            collection.AddScoped(typeof(ICacheConfig), typeof(CacheConfig));

            collection.AddScoped(typeof(IServiceRepository<>), typeof(ServiceRepository<>));
            collection.AddScoped(typeof(IAsyncRepository<>), typeof(EmploymentRepository<>));
            collection.AddScoped(typeof(IUserProfile), typeof(ServiceUserProfile));
            collection.AddScoped(typeof(IServiceLanguages), typeof(ServiceLanguage));
            collection.AddScoped(typeof(IServiceCountry), typeof(ServiceCountry));
            collection.AddScoped(typeof(IServiceIndustry), typeof(ServiceIndustry));
            collection.AddScoped(typeof(IServiceJobFields), typeof(ServiceJobFields));
            collection.AddScoped(typeof(IServiceQualification), typeof(ServiceQualification));
            collection.AddScoped(typeof(IServiceYearsOfExperience), typeof(ServiceYearsOfExperience));
            collection.AddScoped(typeof(IServiceCompany), typeof(ServiceCompany));
            collection.AddScoped(typeof(IServiceJob), typeof(ServiceJob));
            collection.AddScoped(typeof(IServiceJobSeeker), typeof(ServiceJobSeeker));
            collection.AddScoped(typeof(IServiceFavourite), typeof(ServiceFavourite));
            collection.AddScoped(typeof(IServiceApply), typeof(ServiceApply));
            collection.AddScoped(typeof(IServiceJobFair), typeof(ServiceJobFair));
            collection.AddScoped(typeof(IServiceConfigForm), typeof(ServiceConfigForm));

            collection.AddScoped(typeof(IDBNGOType), typeof(DBNGOType));
            collection.AddScoped(typeof(IDBUserProfile), typeof(DBUserProfile));
            collection.AddScoped(typeof(IDBLanguages), typeof(DBLanguages));
            collection.AddScoped(typeof(IDBCountry), typeof(DBCountry));
            collection.AddScoped(typeof(IDBJobFields), typeof(DBJobFields));
            collection.AddScoped(typeof(IDBQualification), typeof(DBQualification));
            collection.AddScoped(typeof(IDBIndustry), typeof(DBIndustry));
            collection.AddScoped(typeof(IDBYearsOfExperience), typeof(DBYearsOfExperience));
            collection.AddScoped(typeof(IDBCompany), typeof(DBCompany));
            collection.AddScoped(typeof(IDBJob), typeof(DBJob));
            collection.AddScoped(typeof(IDBJobSeeker), typeof(DBJobSeeker));
            collection.AddScoped(typeof(IDBFavourite), typeof(DBFavourite));
            collection.AddScoped(typeof(IDBApply), typeof(DBApply));
            collection.AddScoped(typeof(IDBJobFair), typeof(DBJobFair));
            collection.AddScoped(typeof(IDBConfigForm), typeof(DBConfigForm));

            collection.AddScoped(typeof(INotificationEmail), typeof(NotificationEmail));
            collection.AddScoped(typeof(IFile), typeof(ServiceFile));

            _serviceProvider = collection.BuildServiceProvider();
        }
        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
