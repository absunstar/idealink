using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Employment.Services
{
    public class ServiceJobFair : ServiceRepository<JobFair>, IServiceJobFair
    {
        private readonly IDBJobFair _dBJobFair;
        private readonly INotificationEmail _BLNotification;
        public ServiceJobFair(IDBJobFair dBJobFair,
                               INotificationEmail BLNotification) : base(dBJobFair)
        {
            _dBJobFair = dBJobFair;
            _BLNotification = BLNotification;
        }
        public async Task<bool> UpdateFair(JobFair obj)
        {
            if (obj == null)
                return false;

            var fair = await GetById(obj._id);

            obj.CreatedAt = fair.CreatedAt;
            obj.IsActive = fair.IsActive;
            obj.Registered = fair.Registered;

            await _dBJobFair.UpdateObj(obj._id, obj);

            return true;
        }
        public async Task<MongoResultPaged<JobFair>> Search(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var sort = Builders<JobFair>.Sort.Descending(x => x.CreatedAt);
            var filter = Builders<JobFair>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                                                        && x.IsActive == true
                                                        && x.EventDate >= DateTime.Now.Date);

            var lst = await _dBJobFair.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<bool> Register(string JobFairId, JobFairRegisteration obj, Enum.EnumUserTypes role)
        {
            if (await CheckRegister(JobFairId, obj.UserId))
                return true;

            obj.IsAttendance = role == Enum.EnumUserTypes.Admin;

            if (role == Enum.EnumUserTypes.JobSeeker)
            {
                obj.Code = GenerateCode();

                int counter = 5; //try 5 times max to avoid infinite loop
                while (await CheckCode(obj._id, obj.Code) && counter > 0)
                {
                    obj.Code = GenerateCode();
                    counter--;
                }
                var fair = await GetById(JobFairId);
                _BLNotification.EmailAttendanceCode(obj.Code, obj.Name, obj.Email, fair.Name, fair.Location, fair.EventDate, fair.IsOnline);
            }
            return await _dBJobFair.RegisterUser(JobFairId, obj);
        }
        protected long GenerateCode()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            long min = 123456789000;
            long max = 999999999999;

            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
        public async Task<bool> CheckCode(string JobFairId, long Code)
        {
            var filter = Builders<JobFair>.Filter.Where(x => x._id == JobFairId
                                                        && x.IsActive == true
                                                        && x.EventDate > DateTime.Now
                                                        && x.Registered.Any(y => y.Code == Code));
            var result = await _dBJobFair.GetPaged(filter, null, 1, 10);

            return result.totalCount > 0;
        }
        public async Task<bool> CheckRegister(string JobFairId, string UserId)
        {
            var filter = Builders<JobFair>.Filter.Where(x => x._id == JobFairId
                                                        && x.IsActive == true
                                                        && x.EventDate > DateTime.Now
                                                        && x.Registered.Any(y => y.UserId == UserId));
            var result = await _dBJobFair.GetPaged(filter, null, 1, 10);

            return result.totalCount > 0;
        }
        public async Task<bool> SetAttendance(string JobFairId, long Code)
        {
            var filter = Builders<JobFair>.Filter.Where(x => x._id == JobFairId
                                                        && x.IsActive == true
                                                        && x.Registered.Any(y => y.Code == Code));
            var result = await _dBJobFair.GetPaged(filter, null, 1, 1);

            if (result.totalCount == 0)
                return false;

            var obj = result.lstResult[0].Registered.Where(x => x.Code == Code).FirstOrDefault(); ;
            
            FieldDefinition<JobFair> field = "Registered";
            await _dBJobFair.RemoveField(JobFairId, field, obj);

            obj.IsAttendance = true;
            await _dBJobFair.AddField(JobFairId, field, obj);

            return true;
        }
    }
}
