using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Services
{
    public class ServiceYearsOfExperience : ServiceRepository<YearsOfExperience>, IServiceYearsOfExperience
    {
        private readonly IDBYearsOfExperience _dBYearsOfExperience;
        public ServiceYearsOfExperience(IDBYearsOfExperience dBYearsOfExperience) : base(dBYearsOfExperience)
        {
            _dBYearsOfExperience = dBYearsOfExperience;
        }
    }
}
