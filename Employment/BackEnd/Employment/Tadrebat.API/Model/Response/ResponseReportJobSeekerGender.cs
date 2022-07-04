using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseReportJobSeekerGender
    {
        public long Male { get; set; }
        public long Female { get; set; }
        public long Other { get; set; }
    }
}
