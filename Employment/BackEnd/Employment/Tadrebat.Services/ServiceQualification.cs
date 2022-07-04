using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Services
{
    public class ServiceQualification : ServiceRepository<Qualification>, IServiceQualification
    {
        private readonly IDBQualification _dBQualification;
        public ServiceQualification(IDBQualification dBQualification) : base(dBQualification)
        {
            _dBQualification = dBQualification;
        }
    }
}
