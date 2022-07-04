using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelTrainingSearch : ModelPaged
    {
        public string PartnerId { get; set; }
        public string SubPartnerId { get; set; }
        public string TrainingTypeId { get; set; }
        public string TrainingCategoryId { get; set; }
        public string TrainerId { get; set; }
    }
}
