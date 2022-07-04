using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelTrainerCertificateApproval
    {
        [Required]
        public string TrainerId { get; set; }
        [Required]
        public string PartnerId { get; set; }
        [Required]
        public string TrainingCategoryId { get; set; }
    }
}
