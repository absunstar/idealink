using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.Services;

namespace Tadrebat.API.Helpers.AutoMapper
{
    public class HelperMapperUser
    {
        private readonly HelperMapperData helperMapperData;
        private readonly ServiceEntityManagement BLServiceEntity;
        private readonly IDataManagement BLServiceDataManagement;
        public readonly IMapper _mapper;
        private readonly ICacheConfig _BLCacheConfig;
        public HelperMapperUser(IMapper mapper, ServiceEntityManagement _BLServiceEntity, HelperMapperData _helperMapperData, IDataManagement _BLServiceDataManagement, ICacheConfig BLCacheConfig)
        {
            _mapper = mapper;
            helperMapperData = _helperMapperData;
            BLServiceEntity = _BLServiceEntity;
            BLServiceDataManagement = _BLServiceDataManagement;
            _BLCacheConfig = BLCacheConfig;
        }
        public async Task<ResponseUserProfile> MapUser(UserProfile source)
        {
            var destination = new ResponseUserProfile();
            destination = _mapper.Map<UserProfile, ResponseUserProfile>(source);
            //foreach(var obj in source.MyAssignedToAccount)
            //{
            //    var assignee = _mapper.Map<AssignedToAccount, ResponseAssignedToAccount>(obj);
            //    var user = await BLServiceUserProfile.UserProfileGetById(assignee.AccountId);
            //    if(user != null)
            //        assignee.AccountName = user.Name;
            //    destination.MyAssignedToAccount.Add(assignee);
            //}
            foreach (var obj in source.MyPartnerListIds)
            {
                var user = await BLServiceEntity.PartnerGetById(obj);
                if (user != null)
                {
                    var assignee = new ResponseAssignedObj(obj, user.Name);
                    destination.MyPartnerListIds.Add(assignee);
                }
            }
            foreach (var obj in source.MySubPartnerListIds)
            {
                var user = await BLServiceEntity.SubPartnerGetById(obj);
                if (user != null)
                {
                    var assignee = new ResponseAssignedObj(obj, user.Name);
                    destination.MySubPartnerListIds.Add(assignee);
                }
            }
            return destination;
        }
        public async Task<List<ResponseUserProfile>> MapUser(List<UserProfile> source)
        {
            var destination = new List<ResponseUserProfile>();
            foreach (var obj in source)
            {
                destination.Add(await MapUser(obj));
            }
            return destination;
        }
        public async Task<ResponsePaged<ResponseUserProfile>> MapUser(MongoResultPaged<UserProfile> source)
        {
            var destination = new ResponsePaged<ResponseUserProfile>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            foreach (var obj in source.lstResult)
            {
                destination.lstResult.Add(await MapUser(obj));
            }
            return destination;
        }
        public async Task<List<ResponseUserProfileTrainerCertificate>> MapUserTrainerApproval(UserProfile source, List<ResponseTrainingCategory> lstTrainingCategory = null, List<EntityPartner> lstPartners = null)
        {
            if (lstTrainingCategory == null)
            {
                lstTrainingCategory = await GetTrainingCategory();
            }
            if (lstPartners == null)
            {
                lstPartners = await GetPartners();
            }
            var destination = new List<ResponseUserProfileTrainerCertificate>();

            //foreach (var s in source.MyTrainerCertificates.Where(y => y.ExamCount >= _BLCacheConfig.TrainerExamCountCertificate
            //                                                                                        && y.HasCertificate == false
            //                                                                                        && y.IsApproved == false))
            foreach (var s in source.MyTrainerCertificates)
            {
                var obj = new ResponseUserProfileTrainerCertificate();
                obj.TrainerId = source._id;
                obj.TrainerName = source.Name;
                
                var TC = lstTrainingCategory.Where(x => x.Id == s.TrainingCategoryId).FirstOrDefault();
                if (TC != null)
                {
                    obj.TrainingCategoryId = TC.Id;
                    obj.TrainingCategoryName = TC.Name;
                    obj.TrainingTypeId = TC.TrainingType.Id;
                    obj.TrainingTypeName = TC.TrainingType.Name;
                    obj.ExamCount = s.ExamCount;
                    obj.CertificatePath = s.CertificatePath;
                }
                var partner = lstPartners.Where(x => x._id == s.PartnerId).FirstOrDefault();
                if(partner != null)
                {
                    obj.PartnerId = partner._id;
                    obj.PartnerName = partner.Name;
                }
                destination.Add(obj);
            }
            
            return destination;
        }
        public async Task<List<ResponseUserProfileTrainerCertificate>> MapUserTrainerApproval(List<UserProfile> source, List<ResponseTrainingCategory> lstTrainingCategory = null, List<EntityPartner> lstPartners = null)
        {
            if (lstTrainingCategory == null)
            {
                lstTrainingCategory = await GetTrainingCategory();
            }
            if (lstPartners == null)
            {
                lstPartners = await GetPartners();
            }
            var destination = new List<ResponseUserProfileTrainerCertificate>();
            foreach (var obj in source)
            {
                destination.AddRange(await MapUserTrainerApproval(obj));
            }
            return destination;
        }
        public async Task<ResponsePaged<ResponseUserProfileTrainerCertificate>> MapUserTrainerApproval(MongoResultPaged<UserProfile> source, List<ResponseTrainingCategory> lstTrainingCategory = null, List<EntityPartner> lstPartners = null)
        {
            if (lstTrainingCategory == null)
            {
                lstTrainingCategory = await GetTrainingCategory();
            }
            if (lstPartners == null)
            {
                lstPartners = await GetPartners();
            }
            var destination = new ResponsePaged<ResponseUserProfileTrainerCertificate>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            foreach (var obj in source.lstResult)
            {
                destination.lstResult.AddRange(await MapUserTrainerApproval(obj));
            }
            return destination;
        }
        private async Task<List<ResponseTrainingCategory>> GetTrainingCategory()
        {
            var lst = await BLServiceDataManagement.TrainingCategoryListAll("", 1, int.MaxValue);
            var result = await helperMapperData.MapTrainingCategory(lst);

            return result.lstResult;
        }
        private async Task<List<EntityPartner>> GetPartners()
        {
            var lst = await BLServiceEntity.EntityPartnerListAllActiveAnonymous();

            return lst;
        }
    }
}
