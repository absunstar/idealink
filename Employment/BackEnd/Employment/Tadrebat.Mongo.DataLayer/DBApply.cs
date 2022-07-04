using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Mongo.DataLayer
{
    public class DBApply : DBRepositoryMongo<Apply>, IDBApply
    {
        private static string _pDBCollectionName = "Apply";
        public DBApply(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Apply>(_DBCollectionName);
        }
        public async Task<List<ReportApply>> ReportJobSeekerAppliedPerJobCount(DateTime StartDate, DateTime EndDate)
        {
            var filter = _mongoCollection.Aggregate();//.Match(x=>x.IsHired);
            //.Match(e => e.CreatedAt >= StartDate && e.CreatedAt <= EndDate);

            if (StartDate != DateTime.MinValue)
            {
                filter = filter.Match(x => x.CreatedAt >= StartDate);
            }
            if (EndDate != DateTime.MinValue)
            {
                filter = filter.Match(x => x.CreatedAt <= EndDate);

            }

            try
            {
                var result = filter.Group(e => new { e.Job.EntityId, e.Job.Name }, g => new ReportApply() { JobId = g.First().Job.EntityId, JobName = g.First().Job.Name, Count = g.Count() })
                                    .SortBy(x => x.JobName)
                                   .ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
       
    }
}
