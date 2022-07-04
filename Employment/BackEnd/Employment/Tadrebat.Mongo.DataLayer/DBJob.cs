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
    public class DBJob : DBRepositoryMongo<Job>
       , IDBJob
    {
        private static string _pDBCollectionName = "Job";
        public DBJob(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Job>(_DBCollectionName);
        }
        public async Task<bool> UpdateExpiredJobs()
        {
            var filter = Builders<Job>.Filter.Where(x => x.Deadline <= DateTime.Now 
                                                            && x.IsActive == true
                                                            && x.Status == Enum.EnumJobStatus.Approved);

            var update = Builders<Job>.Update.Set("Status", (int)Enum.EnumJobStatus.Closed);

            await _mongoCollection.UpdateManyAsync(filter, update);

            return true;
        }
        public async Task<List<ReportJobCount>> ReportJobPerCompany(string CompanyId, DateTime StartDate, DateTime EndDate, string JobFieldId)
        {
            var filter = _mongoCollection.Aggregate();
                            //.Match(e => e.CreatedAt >= StartDate && e.CreatedAt <= EndDate);

            if (StartDate != DateTime.MinValue)
            {
                filter = filter.Match(x => x.CreatedAt >= StartDate);
            }
            if (EndDate != DateTime.MinValue)
            {
                filter = filter.Match(x => x.CreatedAt <= EndDate);
            }
            if (!string.IsNullOrEmpty(CompanyId) && CompanyId != "-1")
            {
                filter = filter.Match(x => x.Company._id == CompanyId);
            }
            if (!string.IsNullOrEmpty(JobFieldId) && JobFieldId != "-1")
            {
                filter = filter.Match(x => x.JobField._id == JobFieldId);
            }


            try
            {
                var result = filter.Group(e => e.Company, g => new ReportJobCount() { Company = g.Key, Count = g.Count() })
                                    .SortBy(x=>x.Company.Name)
                                   .ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task GetPaged(IEnumerable<JobSearch> filter, SortDefinition<JobSearch> sort, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
