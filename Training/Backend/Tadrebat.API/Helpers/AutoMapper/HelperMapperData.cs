using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.Services;

namespace Tadrebat.API.Helpers.AutoMapper
{
    public class HelperMapperData
    {
        private readonly IDataManagement BLDataManagement;
        private readonly HelperTranslate BLHelperTranslate;
        public readonly IMapper _mapper;
        public HelperMapperData(IMapper mapper, IDataManagement _BLDataManagement, HelperTranslate _BLHelperTranslate)
        {
            _mapper = mapper;
            BLDataManagement = _BLDataManagement;
            BLHelperTranslate = _BLHelperTranslate;
        }
        public async Task<ResponseTrainingCategory> MapTrainingCategory(TrainingCategory source, string Lang = "en")
        {
            var destination = new ResponseTrainingCategory();
            destination = _mapper.Map<TrainingCategory, ResponseTrainingCategory>(source);
            destination = await BLHelperTranslate.MapTrainingCategory(destination, source, Lang);

            var trainingCategory = await BLDataManagement.TrainingTypeGetById(source.TrainingTypeId);
            destination.TrainingType = _mapper.Map<TrainingType, ResponseTrainingType>(trainingCategory);
            destination.TrainingType = await BLHelperTranslate.MapTrainingType(destination.TrainingType, trainingCategory, Lang);
            return destination;
        }
        public async Task<ResponsePaged<ResponseTrainingCategory>> MapTrainingCategory(MongoResultPaged<TrainingCategory> source)
        {
            var destination = new ResponsePaged<ResponseTrainingCategory>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            foreach (var obj in source.lstResult)
            {
                destination.lstResult.Add(await MapTrainingCategory(obj));
            }
            return destination;
        }
        public async Task<List<ResponseTrainingCategory>> MapTrainingCategory(List<TrainingCategory> source, string Lang = "en")
        {
            var destination = new List<ResponseTrainingCategory>();
            foreach (var obj in source)
            {
                destination.Add(await MapTrainingCategory(obj, Lang));
            }
            return destination;
        }
    }
}
