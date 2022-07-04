using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.API.Model.Response;
using Tadrebat.Cache;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.Services;

namespace Tadrebat.API.Helpers.AutoMapper
{
    public class HelperMapperCertificate
    {
        private readonly HelperMapperData helperMapperData;
        private readonly IDataManagement BLServiceDataManagement;
        private readonly ServiceEntityManagement BLEntityManagement;
        private readonly ICacheConfig cacheConfig;

        public readonly IMapper _mapper;
        public HelperMapperCertificate(IMapper mapper,
            ICacheConfig _cacheConfig,
            HelperMapperData _helperMapperData,
            IDataManagement _BLServiceDataManagement,
            ServiceEntityManagement _BLEntityManagement)
        {
            _mapper = mapper;
            cacheConfig = _cacheConfig;
            helperMapperData = _helperMapperData;
            BLServiceDataManagement = _BLServiceDataManagement;
            BLEntityManagement = _BLEntityManagement;
        }
        public async Task<ResponseCertificate> MapCertificate(Certificate source, List<EntityPartner> lstPartner = null, List<ResponseTrainingCategory> lstTrainingCategory = null, string Lang = "en")
        {
            if (lstPartner == null)
            {
                lstPartner = await GetPartner();
            }
            if (lstTrainingCategory == null)
            {
                lstTrainingCategory = await GetTrainingCategory(Lang);
            }

            var destination = new ResponseCertificate();
            destination = _mapper.Map<Certificate, ResponseCertificate>(source);

            if (!string.IsNullOrEmpty(source.TrainingCategoryId))
            {
                var TC = lstTrainingCategory.Where(x => x.Id == source.TrainingCategoryId).FirstOrDefault();
                if (TC != null)
                {
                    destination.TrainingCategoryName = TC.Name;
                    destination.TrainingTypeName = TC.TrainingType.Name;
                }
            }

            if (!string.IsNullOrEmpty(source.PartnerId))
            {
                var partner = lstPartner.Where(x => x._id == source.PartnerId).FirstOrDefault();
                if (partner != null)
                {
                    destination.PartnerName = partner.Name;
                    if (!string.IsNullOrEmpty(source.TrainingCenterId))
                    {
                        var center = partner.TrainingCenters.Where(x => x._id == source.TrainingCenterId).FirstOrDefault();
                        if (center != null)
                        {
                            destination.TrainingCenterName = center.Name;
                        }
                    }
                }
            }

            //SS added file url 
            string FileUrl = cacheConfig.FilesCDN;
            string UpoadFileOnCloud = cacheConfig.UpoadFileOnCloud;
            if (!string.IsNullOrEmpty(source.FileName))
            {
                if (UpoadFileOnCloud == "false")
                    destination.FileName = FileUrl + source.FileName;
            }
            return destination;
        }
        public async Task<List<ResponseCertificate>> MapCertificate(List<Certificate> source, List<EntityPartner> lstPartner = null, List<ResponseTrainingCategory> lstTrainingCategory = null, string Lang = "en")
        {
            if (lstPartner == null)
            {
                lstPartner = await GetPartner();
            }
            if (lstTrainingCategory == null)
            {
                lstTrainingCategory = await GetTrainingCategory(Lang);
            }

            var destination = new List<ResponseCertificate>();
            foreach (var obj in source)
            {
                destination.Add(await MapCertificate(obj, lstPartner, lstTrainingCategory));
            }
            return destination;
        }
        public async Task<ResponsePaged<ResponseCertificate>> MapCertificate(MongoResultPaged<Certificate> source, string Lang = "en")
        {
            var lstTrainingCategory = await GetTrainingCategory(Lang);
            var lstPartner = await GetPartner();

            var destination = new ResponsePaged<ResponseCertificate>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            destination.lstResult = await MapCertificate(source.lstResult, lstPartner, lstTrainingCategory);

            return destination;
        }
        private async Task<List<ResponseTrainingCategory>> GetTrainingCategory(string Lang)
        {
            var lst = await BLServiceDataManagement.TrainingCategoryListAll("", 1, int.MaxValue);
            var result = await helperMapperData.MapTrainingCategory(lst.lstResult, Lang);

            return result;
        }
        private async Task<List<EntityPartner>> GetPartner()
        {
            var lst = await BLEntityManagement.PartnerListAll("", "", 1, int.MaxValue);

            return lst.lstResult;
        }
    }
}
