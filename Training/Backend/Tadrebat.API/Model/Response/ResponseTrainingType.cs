using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseTrainingType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ResponseTrainingType TrainingType { get; set; }
    }
}
