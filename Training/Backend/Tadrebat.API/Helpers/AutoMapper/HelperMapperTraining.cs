using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.Services;

namespace Tadrebat.API.Helpers.AutoMapper
{
    public class HelperMapperTraining
    {
        private readonly ITraining BLServiceTraining;
        private readonly IDataManagement BLServiceDatamanagement;
        private readonly ServiceEntityManagement BLServiceEntityManagement;
        private readonly IUserProfile BLUserProfile;
        public readonly IMapper _mapper;
        public HelperMapperTraining(IMapper mapper, ITraining _BLServiceTraining, IDataManagement _BLServiceDatamanagement
                                    , ServiceEntityManagement _BLServiceEntityManagement, IUserProfile _BLUserProfile)
        {
            _mapper = mapper;
            BLServiceTraining = _BLServiceTraining;
            BLServiceDatamanagement = _BLServiceDatamanagement;
            BLServiceEntityManagement = _BLServiceEntityManagement;
            BLUserProfile = _BLUserProfile;
        }
        public async Task<ResponseTraining> MapTraining(Training source, string Lang = "en")
        {
            var destination = new ResponseTraining();
            destination = _mapper.Map<Training, ResponseTraining>(source);

            destination.CanEdit = DateTime.Now <= source.EndDate;

            var trainingCategory = await BLServiceDatamanagement.TrainingCategoryGetById(source.TrainingCategoryId);
            var trainingType = await BLServiceDatamanagement.TrainingTypeGetById(source.TrainingTypeId);
            var user = await BLUserProfile.UserProfileGetById(source.TrainerId);

            switch (Lang)
            {
                case "en":
                    destination.TrainingCategoryId = new ResponseItemDetails(trainingCategory._id, trainingCategory.Name);
                    destination.TrainingTypeId = new ResponseItemDetails(trainingType._id, trainingType.Name);
                    break;
                case "ar":
                    destination.TrainingCategoryId = new ResponseItemDetails(trainingCategory._id, trainingCategory.Name2);
                    destination.TrainingTypeId = new ResponseItemDetails(trainingType._id, trainingType.Name2);
                    break;
                case "fr":
                    destination.TrainingCategoryId = new ResponseItemDetails(trainingCategory._id, trainingCategory.Name3);
                    destination.TrainingTypeId = new ResponseItemDetails(trainingType._id, trainingType.Name3);
                    break;
            }
            if (user != null)
            {
                destination.TrainerDetails.Name = user.Name;
                destination.TrainerDetails.Id = source.TrainerId;
            }
            return destination;
        }
        public async Task<List<ResponseTraining>> MapTraining(List<Training> source, string Lang = "en")
        {
            var destination = new List<ResponseTraining>();
            foreach (var obj in source)
            {
                destination.Add(await MapTraining(obj, Lang));
            }
            return destination;
        }
        public async Task<ResponsePaged<ResponseTraining>> MapTraining(MongoResultPaged<Training> source, string Lang = "en")
        {
            var destination = new ResponsePaged<ResponseTraining>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            destination.lstResult = await MapTraining(source.lstResult, Lang);

            return destination;
        }
        public async Task<Training> UpdateTraining(Training model)
        {
            var partner = await BLServiceEntityManagement.PartnerGetById(model.PartnerId._id);
            model.PartnerId.Name = partner.Name;
            model.TrainingCenterId.Name = partner.TrainingCenters.Where(x => x._id == model.TrainingCenterId._id).FirstOrDefault().Name;

            var subpartner = await BLServiceEntityManagement.SubPartnerGetById(model.SubPartnerId._id);
            model.SubPartnerId.Name = subpartner.Name;

            return model;
        }
    }
}
