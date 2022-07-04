using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model
{
    public class ResponseTraineeCertificates
    {
        public string TrainingName { get; set; }
        public string Date { get; set; }
        public string TrainingDescription { get; set; }
        public string FilePath { get; set; }

    }
}
