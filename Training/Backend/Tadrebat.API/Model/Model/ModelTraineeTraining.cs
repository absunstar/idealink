using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelTraineeTraining
    {
        [Required]
        public string TraineeId { get; set; }
        [Required]
        public string TrainingId { get; set; }
    }
}
