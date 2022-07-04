using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.Services;

namespace Tadrebat.API.Helpers.AutoMapper
{
    public class HelperMapperEntity
    {
        private readonly ServiceEntityManagement BLEntityManagement;
        private readonly IUserProfile BLUserProfile;
        public readonly IMapper _mapper;
        public HelperMapperEntity(IMapper mapper, ServiceEntityManagement _BLEntityManagement
                                , IUserProfile _BLUserProfile)
        {
            _mapper = mapper;
            BLEntityManagement = _BLEntityManagement;
            BLUserProfile = _BLUserProfile;
        }
        #region TrainingCenter
        public async Task<ResponseEntityTrainingCenter> MapTrainingCenter(EntityTrainingCenter source)
        {
            var destination = new ResponseEntityTrainingCenter();
            destination = _mapper.Map<EntityTrainingCenter, ResponseEntityTrainingCenter>(source);
            //source.PartnerIds.ForEach(async (obj) =>
            //{
            //    var result = await BLEntityManagement.PartnerGetById(obj);
            //    var response = _mapper.Map<EntityPartner, ResponseEntityPartner>(result);
            //    destination.Partners.Add(response);
            //});
            return destination;
        }
        public async Task<List<ResponseEntityTrainingCenter>> MapTrainingCenter(List<EntityTrainingCenter> source)
        {
            var destination = new List<ResponseEntityTrainingCenter>();
            foreach (var obj in source)
            {
                destination.Add(await MapTrainingCenter(obj));
            }
            return destination;
        }
        public async Task<ResponsePaged<ResponseEntityTrainingCenter>> MapTrainingCenter(MongoResultPaged<EntityTrainingCenter> source)
        {
            var destination = new ResponsePaged<ResponseEntityTrainingCenter>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            foreach (var obj in source.lstResult)
            {
                destination.lstResult.Add(await MapTrainingCenter(obj));
            }
            return destination;
        }
        #endregion
        #region SubPartner
        public async Task<ResponseEntitySubPartner> MapSubParnter(EntitySubPartner source)
        {
            var destination = new ResponseEntitySubPartner();
            destination = _mapper.Map<EntitySubPartner, ResponseEntitySubPartner>(source);
            source.PartnerIds.ForEach(async (obj) =>
            {
                var result = await BLEntityManagement.PartnerGetById(obj);
                var response = _mapper.Map<EntityPartner, ResponseEntityPartner>(result);
                destination.Partners.Add(response);
            });
            if (destination.Partners.Count > 0)
            {
                var lstTrainingCenter = new List<ResponseEntityTrainingCenter>();
                foreach (var obj in destination.Partners)
                {
                    if (obj.TrainingCenters != null)
                        lstTrainingCenter.AddRange(obj.TrainingCenters);
                }
                source.TrainingCenterIds.ForEach(async (obj) =>
                {
                    var result = lstTrainingCenter.Where(x => x.Id == obj).FirstOrDefault();
                    if (result != null)
                        destination.TrainingCenters.Add(result);
                });
            }
            source.MemberCanAccessIds.ForEach(async (obj) =>
            {
                var result = await BLUserProfile.UserProfileGetById(obj);
                var response = _mapper.Map<UserProfile, ResponseUserProfile>(result);
                destination.MemberSubPartners.Add(response);
            });
            return destination;
        }
        public async Task<List<ResponseEntitySubPartner>> MapSubParnter(List<EntitySubPartner> source)
        {
            var destination = new List<ResponseEntitySubPartner>();
            foreach (var obj in source)
            {
                destination.Add(await MapSubParnter(obj));
            }
            return destination;
        }
        public async Task<ResponsePaged<ResponseEntitySubPartner>> MapSubParnter(MongoResultPaged<EntitySubPartner> source)
        {
            var destination = new ResponsePaged<ResponseEntitySubPartner>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            foreach (var obj in source.lstResult)
            {
                destination.lstResult.Add(await MapSubParnter(obj));
            }
            return destination;
        }
        #endregion
        #region Partner
        public async Task<ResponseEntityPartner> MapPartner(EntityPartner source, bool addPartnerAccount = false)
        {
            var destination = new ResponseEntityPartner();
            destination = _mapper.Map<EntityPartner, ResponseEntityPartner>(source);
            source.MemberCanAccessIds.ForEach(async (obj) =>
            {
                var result = await BLUserProfile.UserProfileGetById(obj);
                if (addPartnerAccount)
                {
                    if (result.Type == (int)EnumUserTypes.Partner)
                    {
                        var response = _mapper.Map<UserProfile, ResponseUserProfile>(result);
                        destination.Members.Add(response);
                    }
                }
            });
            return destination;
        }
        public async Task<List<ResponseEntityPartner>> MapPartner(List<EntityPartner> source, bool addPartnerAccount = false)
        {
            var destination = new List<ResponseEntityPartner>();
            foreach (var obj in source)
            {
                destination.Add(await MapPartner(obj, addPartnerAccount));
            }
            return destination;
        }
        public async Task<ResponsePaged<ResponseEntityPartner>> MapPartner(MongoResultPaged<EntityPartner> source, bool addPartnerAccount = false)
        {
            var destination = new ResponsePaged<ResponseEntityPartner>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            destination.lstResult = await MapPartner(source.lstResult, addPartnerAccount);

            return destination;
        }
        #endregion
        //public async Task<ResponseUserProfile> MapUserProfile(UserProfile source)
        //{
        //    var user = await BLUserProfile.UserProfileGetById(obj);
        //    var destination = new ResponseUserProfile();
        //    destination = _mapper.Map<UserProfile, ResponseUserProfile>(source);
        //    source.Members.ForEach(async (obj) =>
        //    {

        //        var response = _mapper.Map<UserProfile, ResponseUserProfile>(result);
        //        destination.Members.Add(response);
        //    });
        //    return destination;
        //}
    }
}
