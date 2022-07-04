using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Services
{
    public class ServiceLanguage : ServiceRepository<Languages>,IServiceLanguages
    {
        private readonly IDBLanguages _dBLanguages;
        public ServiceLanguage(IDBLanguages dBLanguages):base(dBLanguages)
        {
            _dBLanguages = dBLanguages;
        }
    }
}
