using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseTrainingCategory
    {
        public ResponseTrainingCategory()
        {
            Course = new List<ResponseCourse>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ResponseTrainingType TrainingType { get; set; }
        public List<ResponseCourse> Course { get; set; }
    }
}
