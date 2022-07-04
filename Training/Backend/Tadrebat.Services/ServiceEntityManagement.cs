using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;
using System.Linq;
using SQL;

namespace Tadrebat.Services
{
    public class ServiceEntityManagement //: IEntityManagement
    {
        private readonly IDBEntityPartner _dBEntityPartner;
        private readonly IDBEntitySubPartner _dBEntitySubPartner;
        private readonly IDBEntityTrainingCenter _dBEntityTrainingCenter;
        private readonly ServiceSQL _BLServiceSQL;
        public ServiceEntityManagement(IDBEntityPartner dBEntityPartner
                                , IDBEntitySubPartner dBEntitySubPartner,
                                 ServiceSQL BLServiceSQL
                                , IDBEntityTrainingCenter dBEntityTrainingCenter)
        {
            _dBEntityPartner = dBEntityPartner;
            _dBEntitySubPartner = dBEntitySubPartner;
            _dBEntityTrainingCenter = dBEntityTrainingCenter;
            _BLServiceSQL = BLServiceSQL;
        }
        public static string RandomString(int length)
        {
            Random random = new Random();
            string strPassword = "";
            string chars = "qwertyuioplkjhgfdsazxcvbnm";
            
            strPassword = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            
            chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            strPassword += new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            chars = "!@#$%^&*()./,][+_)|?><";
            strPassword += new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            strPassword = new string(Enumerable.Repeat(strPassword, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());


            return strPassword;
        }
        #region Partner
        public async Task<EntityPartner> PartnerGetById(string Id)
        {
            return await _dBEntityPartner.GetById(Id);
        }
        public async Task<bool> PartnerCreate(EntityPartner obj)
        {
            if (string.IsNullOrEmpty(obj.Name))
                return false;
            obj.UserName = "P" + obj._id;
            obj.Password = RandomString(10);
            
            await _dBEntityPartner.AddAsync(obj);

            try
            {
                await _BLServiceSQL.GeneratePartnerAccountView(obj._id, obj.Password);
            }
            catch (Exception ex)
            {

                ServiceHelper.Log(ex.InnerException + "::::::::" + ex.Message);
            }

            return true;
        }
        public async Task<bool> PartnerUpdate(EntityPartner obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj._id))
                return false;
            var p = await PartnerGetById(obj._id);
            p.Name = obj.Name;
            p.MaxHours = obj.MaxHours;
            p.MinHours = obj.MinHours;
            p.Phone = obj.Phone;
            p.UserName = obj.UserName;
            p.Password = obj.Password;

            await _dBEntityPartner.UpdateObj(obj._id, p);

            return true;
        }
        public async Task<bool> PartnerDeActivate(string Id)
        {
            var obj = await PartnerGetById(Id);
            if (obj == null)
                return false;

            await _dBEntityPartner.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> PartnerActivate(string Id)
        {
            var obj = await PartnerGetById(Id);
            if (obj == null)
                return false;

            await _dBEntityPartner.ActivateAsync(Id);

            return true;
        }
        public async Task<List<EntityPartner>> EntityPartnerListAllActiveAnonymous()
        {
            var sort = Builders<EntityPartner>.Sort.Ascending(x => x.Name);
            var lst = await _dBEntityPartner.ListActive(sort);

            return lst;
        }
        public async Task<List<EntityPartner>> PartnerListActive(string UserId, string query = "")
        {
            var filter = Builders<EntityPartner>.Filter.Where(x => x.MemberCanAccessIds.Contains(UserId));
            var sort = Builders<EntityPartner>.Sort.Ascending(x => x.Name);
            if (!string.IsNullOrEmpty(query))
            {
                filter = filter & Builders<EntityPartner>.Filter.Where(x => x.Name.Contains(query));
            }
            List<EntityPartner> lst = new List<EntityPartner>();
            if (UserId != "")
                lst = await _dBEntityPartner.ListActive(filter, sort);
            else
                lst = await _dBEntityPartner.ListActive(sort);

            return lst;
        }
        public async Task<List<EntityPartner>> EntityPartnerGetMy(string UserId)
        {
            var filter = Builders<EntityPartner>.Filter.Where(x => x.MemberCanAccessIds.Contains(UserId));
            var sort = Builders<EntityPartner>.Sort.Descending(x => x.Name);

            var lst = await _dBEntityPartner.ListActive(filter, sort);

            return lst;
        }
        public async Task<MongoResultPaged<EntityPartner>> PartnerListAll(string filterText, string UserId = "", int pageNumber = 1, int PageSize = 15)
        {
            var lst = await _dBEntityPartner.ListAllSearch(filterText, UserId, pageNumber, PageSize);
            return lst;
        }
        public async Task<bool> IsPartnerExist(string Id)
        {
            var obj = await _dBEntityPartner.GetById(Id);
            return obj != null;
        }
        public async Task<bool> PartnerAddMember(string PartnerId, string UserId)
        {
            FieldDefinition<EntityPartner> field = "MemberCanAccessIds";
            await _dBEntityPartner.AddField(PartnerId, field, UserId);
            return true;
        }
        public async Task<bool> PartnerRemoveMember(string PartnerId, string UserId)
        {
            FieldDefinition<EntityPartner> field = "MemberCanAccessIds";
            await _dBEntityPartner.RemoveField(PartnerId, field, UserId);

            return true;
        }
        public async Task<bool> PartnerAddTrainingCenter(EntityTrainingCenter obj, string PartnerId)
        {
            FieldDefinition<EntityPartner> field = "TrainingCenters";
            if (obj._id == null)
                obj._id = ObjectId.GenerateNewId().ToString();

            await _dBEntityPartner.AddField(PartnerId, field, obj);
            return true;
        }
        public async Task<bool> PartnerRemoveTrainingCenter(EntityTrainingCenter obj, string PartnerId)
        {
            FieldDefinition<EntityPartner> field = "TrainingCenters";
            await _dBEntityPartner.RemoveField(PartnerId, field, obj);

            return true;
        }
        #endregion
        #region EntitySubPartner
        public async Task<EntitySubPartner> SubPartnerGetById(string Id, string UserId = "-1")
        {
            var obj = await _dBEntitySubPartner.GetById(Id);
            return await SubPartnerUpdatePartnersToMyListOnly(obj, UserId);
        }
        public async Task<bool> SubPartnerCreate(EntitySubPartner obj)
        {
            if (string.IsNullOrEmpty(obj.Name))
                return false;

            var p = new EntitySubPartner();
            p.Name = obj.Name;
            p.Phone = obj.Phone;
            p._id = obj._id;

            await _dBEntitySubPartner.AddAsync(p);

            return true;
        }
        public async Task<bool> SubPartnerUpdate(EntitySubPartner obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj._id))
                return false;

            var sub = await SubPartnerGetById(obj._id);
            if(sub == null)
                return false;

            sub.Name = obj.Name;
            sub.Phone = obj.Phone;

            await _dBEntitySubPartner.UpdateObj(obj._id, sub);

            return true;
        }
        public async Task<bool> SubPartnerDeActivate(string Id)
        {
            var obj = await SubPartnerGetById(Id);
            if (obj == null)
                return false;

            await _dBEntitySubPartner.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> SubPartnerActivate(string Id)
        {
            var obj = await SubPartnerGetById(Id);
            if (obj == null)
                return false;

            await _dBEntitySubPartner.ActivateAsync(Id);

            return true;
        }
        public async Task<List<EntitySubPartner>> SubPartnerListActive(string UserId, string query, List<string> PartnerIds)
        {
            if (PartnerIds == null)
                PartnerIds = new List<string>();

            var filter = Builders<EntitySubPartner>.Filter.Where(x => x.IsActive == true);

            if (!string.IsNullOrEmpty(UserId))
                filter = filter & Builders<EntitySubPartner>.Filter.Where(x => x.MemberCanAccessIds.Contains(UserId));

            var sort = Builders<EntitySubPartner>.Sort.Descending(x => x.Name);
            if (!string.IsNullOrEmpty(query))
            {
                filter = filter & Builders<EntitySubPartner>.Filter.Where(x => x.Name.Contains(query));
            }
            if (PartnerIds.Count > 0)
            {
                filter = filter & Builders<EntitySubPartner>.Filter.AnyIn(x => x.PartnerIds, PartnerIds);
            }
            List<EntitySubPartner> lst = new List<EntitySubPartner>();
            //if (UserId != "")
            lst = await _dBEntitySubPartner.ListActive(filter, sort);
            //else
            //    lst = await _dBEntitySubPartner.ListActive(sort);
            return lst;
        }
        public async Task<List<EntitySubPartner>> EntitySubPartnerGetMy(string UserId)
        {
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x.MemberCanAccessIds.Contains(UserId));

            var sort = Builders<EntitySubPartner>.Sort.Descending(x => x.Name);
            var lst = await _dBEntitySubPartner.ListActive(filter, sort);

            return lst;
        }
        public async Task<List<EntitySubPartner>> EntitySubPartnerGetMyByPartnerId(string UserId, string PartnerId)
        {
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x.PartnerIds.Contains(PartnerId));

            if (!string.IsNullOrEmpty(UserId))
                filter = filter & Builders<EntitySubPartner>.Filter.Where(x => x.MemberCanAccessIds.Contains(UserId));

            var sort = Builders<EntitySubPartner>.Sort.Descending(x => x.Name);
            var lst = await _dBEntitySubPartner.ListActive(filter, sort);

            return lst;
        }
        public async Task<MongoResultPaged<EntitySubPartner>> SubPartnerListAll(string filterText, string UserId, int pageNumber = 1, int PageSize = 15)
        {
            var lst = await _dBEntitySubPartner.ListAllSearch(filterText, UserId, pageNumber, PageSize);
            for (int i = 0; i < lst.lstResult.Count; i++)
            {
                lst.lstResult[i] = await SubPartnerUpdatePartnersToMyListOnly(lst.lstResult[i], UserId);
            }
            return lst;
        }
        public async Task<bool> IsSubPartnerExist(string Id)
        {
            var obj = await _dBEntitySubPartner.GetById(Id);
            return obj != null;
        }
        protected async Task<EntitySubPartner> SubPartnerUpdatePartnersToMyListOnly(EntitySubPartner obj, string UserId)
        {
            //if userId = -1 dont update
            //if userId = "" that is admin, dont update
            if (UserId == "-1" || UserId == "")
                return obj;

            //if obj is null return null
            if (obj != null)
            {
                var myPartnerList = await PartnerListActive(UserId);
                var myPartnerIds = myPartnerList.Select(y => y._id).ToList();
                obj.PartnerIds = obj.PartnerIds.Where(x => myPartnerIds.Contains(x)).ToList();
            }
            return obj;
        }
        //Add Account to SubPartner Entity
        public async Task<bool> SubPartnerAddMember(string SubPartnerId, string UserId)
        {
            FieldDefinition<EntitySubPartner> field = "MemberCanAccessIds";
            await _dBEntitySubPartner.AddField(SubPartnerId, field, UserId);
            return true;
        }
        public async Task<bool> SubPartnerAddMember(string SubPartnerId, List<string> UserId)
        {
            FieldDefinition<EntitySubPartner> field = "MemberCanAccessIds";
            await _dBEntitySubPartner.AddFieldList(SubPartnerId, field, UserId.ToArray());
            return true;
        }
        public async Task<bool> SubPartnerRemoveMember(string SubPartnerId, string UserId)
        {
            FieldDefinition<EntitySubPartner> field = "MemberCanAccessIds";
            await _dBEntitySubPartner.RemoveField(SubPartnerId, field, UserId);
            return true;
        }
        public async Task<bool> SubPartnerRemoveMember(string SubPartnerId, List<string> UserId)
        {
            FieldDefinition<EntitySubPartner> field = "MemberCanAccessIds";
            await _dBEntitySubPartner.RemoveFieldList(SubPartnerId, field, UserId.ToArray());
            return true;
        }
        public async Task<List<EntitySubPartner>> SubPartnerGetByPartnerId(string PartnerId)
        {
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x.PartnerIds.Contains(PartnerId));
            var lst = await _dBEntitySubPartner.GetPaged(filter, null, 1, int.MaxValue);
            return lst.lstResult;
        }
        public async Task<bool> SubPartnerAddMemberByPartnerId(string PartnerId, string UserId)
        {
            //Get all subpartner by PartnerId and add user to them
            FieldDefinition<EntitySubPartner> field = "MemberCanAccessIds";
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x.PartnerIds.Contains(PartnerId));
            await _dBEntitySubPartner.AddField(filter, field, UserId);
            return true;
        }
        public async Task<bool> SubPartnerRemoveMemberByPartnerId(string PartnerId, string UserId)
        {
            FieldDefinition<EntitySubPartner> field = "MemberCanAccessIds";
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x.PartnerIds.Contains(PartnerId));
            await _dBEntitySubPartner.RemoveField(filter, field, UserId);
            return true;
        }
        public async Task<bool> SubPartnerAddTrainingCenter(string TrainingCenterId, string SubPartnerId)
        {
            FieldDefinition<EntitySubPartner> field = "TrainingCenterIds";
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x._id == SubPartnerId);
            await _dBEntitySubPartner.AddField(filter, field, TrainingCenterId);
            return true;
        }
        public async Task<bool> SubPartnerRemoveTrainingCenter(string TrainingCenterId, string SubPartnerId)
        {
            FieldDefinition<EntitySubPartner> field = "TrainingCenterIds";
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x._id == SubPartnerId);
            await _dBEntitySubPartner.RemoveField(filter, field, TrainingCenterId);
            return true;
        }
        public async Task<bool> SubPartnerAddPartnerTrainingCenter(string SubPartnerId, string PartnerId)
        {
            FieldDefinition<EntitySubPartner> field = "TrainingCenterIds";
            //var filter = Builders<EntitySubPartner>.Filter.Where(x => x._id == SubPartnerId);
            var obj = await PartnerGetById(PartnerId);

            if (obj == null)
                return false;
            await _dBEntitySubPartner.AddFieldList(SubPartnerId, field, obj.TrainingCenters.Where(x => x.IsActive == true).Select(x => x._id).ToArray());
            return true;
        }
        public async Task<bool> SubPartnerRemovePartnerTrainingCenter(string SubPartnerId, string PartnerId)
        {
            FieldDefinition<EntitySubPartner> field = "TrainingCenterIds";
            //var filter = Builders<EntitySubPartner>.Filter.Where(x => x._id == SubPartnerId);
            var obj = await PartnerGetById(PartnerId);

            if (obj == null)
                return false;

            await _dBEntitySubPartner.RemoveFieldList(SubPartnerId, field, obj.TrainingCenters.Select(x => x._id).ToArray());
            return true;
        }
        //Add Partner Entity to SubPartnerEntty
        public async Task<bool> SubPartnerAddPartner(string SubPartnerId, string PartnerId)
        {
            FieldDefinition<EntitySubPartner> field = "PartnerIds";
            await _dBEntitySubPartner.AddField(SubPartnerId, field, PartnerId);
            return true;
        }
        public async Task<bool> SubPartnerRemovePartner(string SubPartnerId, string PartnerId)
        {
            FieldDefinition<EntitySubPartner> field = "PartnerIds";
            await _dBEntitySubPartner.RemoveField(SubPartnerId, field, PartnerId);
            return true;
        }
        #endregion
        #region TrainingCenter
        //public async Task<EntityTrainingCenter> TrainingCenterGetById(string Id)
        //{
        //    return await _dBEntityTrainingCenter.GetById(Id);
        //}
        //public async Task<bool> TrainingCenterCreate(EntityTrainingCenter obj)
        //{
        //    if (string.IsNullOrEmpty(obj.Name))
        //        return false;

        //    await _dBEntityTrainingCenter.AddAsync(obj);

        //    return true;
        //}
        public async Task<bool> TrainingCenterUpdate(string PartnerId, EntityTrainingCenter obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj._id))
                return false;

            var filter = Builders<EntityPartner>.Filter.ElemMatch(x => x.TrainingCenters, y => y._id == obj._id);
            var update = Builders<EntityPartner>.Update.Set(x => x.TrainingCenters[-1].Name, obj.Name)
                                                        .Set(x => x.TrainingCenters[-1].Phone, obj.Phone);

            await _dBEntityPartner.UpdateAsync(filter, update);

            return true;
        }
        public async Task<bool> TrainingCenterDeActivate(string PartnerId, string Id)
        {
            var filter = Builders<EntityPartner>.Filter.ElemMatch(x => x.TrainingCenters, y => y._id == Id);
            var update = Builders<EntityPartner>.Update.Set(x => x.TrainingCenters[-1].IsActive, false);

            await _dBEntityPartner.UpdateAsync(filter, update);

            return true;
        }
        public async Task<bool> TrainingCenterActivate(string PartnerId, string Id)
        {
            var filter = Builders<EntityPartner>.Filter.ElemMatch(x => x.TrainingCenters, y => y._id == Id);
            var update = Builders<EntityPartner>.Update.Set(x => x.TrainingCenters[-1].IsActive, true);

            await _dBEntityPartner.UpdateAsync(filter, update);

            return true;
        }
        public async Task<List<EntityTrainingCenter>> TrainingCenterListActive()
        {
            var sort = Builders<EntityTrainingCenter>.Sort.Descending(x => x.Name);
            var lst = await _dBEntityTrainingCenter.ListActive(sort);
            return lst;
        }
        public async Task<MongoResultPaged<EntityTrainingCenter>> TrainingCenterListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var lst = await _dBEntityTrainingCenter.ListAllSearch(filterText, pageNumber, PageSize);
            return lst;
        }

        public async Task<bool> IsTrainingCenterExist(string Id)
        {
            var obj = await _dBEntityTrainingCenter.GetById(Id);
            return obj != null;
        }
        #endregion
    }
}
