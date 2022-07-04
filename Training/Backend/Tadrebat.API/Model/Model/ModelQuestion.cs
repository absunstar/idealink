using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelQuestion
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Difficulty { get; set; }
        public string TrainingCategoryId { get; set; }
        public string TrainingTypeId { get; set; }
        public List<ModelAnswer> Answer { get; set; }
    }
    public class ModelAnswer 
    {
        public string Name { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }
}
