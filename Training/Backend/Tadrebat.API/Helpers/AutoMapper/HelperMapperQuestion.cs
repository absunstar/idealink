using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;

namespace Tadrebat.API.Helpers.AutoMapper
{
    public class HelperMapperQuestion
    {
        private readonly HelperMapperData helperMapperData;
        private readonly IDataManagement BLServiceDataManagement;

        public readonly IMapper _mapper;
        public HelperMapperQuestion(IMapper mapper, HelperMapperData _helperMapperData, IDataManagement _BLServiceDataManagement)
        {
            _mapper = mapper;
            helperMapperData = _helperMapperData;
            BLServiceDataManagement = _BLServiceDataManagement;
        }
        public async Task<ResponseQuestion> MapQuestion(Question source, List<ResponseTrainingCategory> lstTrainingCategory = null, string Lang = "en")
        {
            if (lstTrainingCategory == null)
            {
                lstTrainingCategory = await GetTrainingCategory(Lang);
            }

            var destination = new ResponseQuestion();
            destination = _mapper.Map<Question, ResponseQuestion>(source);

            var TC = lstTrainingCategory.Where(x => x.Id == source.TrainingCategoryId).FirstOrDefault();
            if(TC != null)
            {
                destination.TrainingCategoryName = TC.Name;
                destination.TrainingTypeName = TC.TrainingType.Name;
            }

            return destination;
        }
        public async Task<List<ResponseQuestion>> MapQuestion(List<Question> source, List<ResponseTrainingCategory> lstTrainingCategory = null, string Lang = "en")
        {
            if(lstTrainingCategory == null)
            {
                lstTrainingCategory = await GetTrainingCategory(Lang);
            }

            var destination = new List<ResponseQuestion>();
            foreach (var obj in source)
            {
                destination.Add(await MapQuestion(obj, lstTrainingCategory));
            }
            return destination;
        }
        public async Task<ResponsePaged<ResponseQuestion>> MapQuestion(MongoResultPaged<Question> source, string Lang = "en")
        {
            var lstTrainingCategory = await GetTrainingCategory(Lang);

            var destination = new ResponsePaged<ResponseQuestion>();
            destination.pageSize = source.pageSize;
            destination.totalCount = source.totalCount;
            destination.lstResult = await MapQuestion(source.lstResult, lstTrainingCategory);

            return destination;
        }
        private async Task<List<ResponseTrainingCategory>> GetTrainingCategory(string Lang)
        {
            var lst = await BLServiceDataManagement.TrainingCategoryListAll("", 1, int.MaxValue);
            var result = await helperMapperData.MapTrainingCategory(lst.lstResult, Lang);

            return result;
        }
    }
}
