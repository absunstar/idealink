using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.MongoDB.Interface
{
    public interface IDBApply : IRepositoryMongo<Apply>
    {
        Task<List<ReportApply>> ReportJobSeekerAppliedPerJobCount(DateTime StartDate, DateTime EndDate);
    }
}
