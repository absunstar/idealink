using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelFilterQuestions : ModelPaged
    {
        public string TrainingCagtegoryId { get; set; }
        public string TrainingTypeId { get; set; }
    }
}
