using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Model
{
    public class ModelTraining
    {
        public ModelTraining()
        {
            days = new List<string>();
        }
        public string Id { get; set; }
        public string PartnerId { get; set; }
        public string SubPartnerId { get; set; }
        public string TrainingCenterId { get; set; }
        public EnumTrainingType Type { get; set; }
        public string TrainerId { get; set; }
        public string TrainingTypeId { get; set; }
        public string TrainingCategoryId { get; set; }
        public string CityId { get; set; }
        public string AreaId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> days { get; set; }
        public bool IsOnline { get; set; }
    }
}
