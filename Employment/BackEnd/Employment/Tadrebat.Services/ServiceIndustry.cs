using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Services
{
    public class ServiceIndustry : ServiceRepository<Industry>, IServiceIndustry
    {
        private readonly IDBIndustry _dBIndustry;
        public ServiceIndustry(IDBIndustry dBIndustry) : base(dBIndustry)
        {
            _dBIndustry = dBIndustry;
        }
    }
}
