using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employment.API.Model.Response;
using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.Services;

namespace Employment.API.Helpers.AutoMapper
{
    public class HelperMapperData
    {
        private readonly IDataManagement BLDataManagement;
        public readonly IMapper _mapper;
        public HelperMapperData(IMapper mapper, IDataManagement _BLDataManagement)
        {
            _mapper = mapper;
            BLDataManagement = _BLDataManagement;
        }
        //public async Task<ResponseTrainingCategory> MapTrainingCategory(TrainingCategory source)
        //{
        //    var destination = new ResponseTrainingCategory();
        //    destination = _mapper.Map<TrainingCategory, ResponseTrainingCategory>(source);
        //    var trainingCategory = await BLDataManagement.TrainingTypeGetById(source.TrainingTypeId);
        //    destination.TrainingType = _mapper.Map<TrainingType, ResponseTrainingType>(trainingCategory); ;
        //    return destination;
        //}
        //public async Task<ResponsePaged<ResponseTrainingCategory>> MapTrainingCategory(MongoResultPaged<TrainingCategory> source)
        //{
        //    var destination = new ResponsePaged<ResponseTrainingCategory>();
        //    destination.pageSize = source.pageSize;
        //    destination.totalCount = source.totalCount;
        //    foreach (var obj in source.lstResult)
        //    {
        //        destination.lstResult.Add(await MapTrainingCategory(obj));
        //    }
        //    return destination;
        //}
        //public async Task<List<ResponseTrainingCategory>> MapTrainingCategory(List<TrainingCategory> source)
        //{
        //    var destination = new List<ResponseTrainingCategory>();
        //    foreach (var obj in source)
        //    {
        //        destination.Add(await MapTrainingCategory(obj));
        //    }
        //    return destination;
        //}
    }
}
