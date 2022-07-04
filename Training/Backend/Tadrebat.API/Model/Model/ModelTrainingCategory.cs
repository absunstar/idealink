﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelTrainingCategory
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string TrainingTypeId { get; set; }
    }
}
