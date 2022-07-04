using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseQuestion
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Difficulty { get; set; }
        public string TrainingCategoryId { get; set; }
        public string TrainingCategoryName { get; set; }
        public string TrainingTypeId { get; set; }
        public string TrainingTypeName { get; set; }
        public List<ResponseAnswer> Answer { get; set; }
    }
    public class ResponseAnswer
    {
        public string Name { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }
}