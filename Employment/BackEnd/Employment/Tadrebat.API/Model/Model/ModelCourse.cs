using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelCourse
    {
        public string Id { get; set; }
        public string TrainingCategoryId { get; set; }
        public string Name { get; set; }
    }
}
