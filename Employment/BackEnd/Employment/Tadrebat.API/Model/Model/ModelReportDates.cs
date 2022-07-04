using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelReportDates
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class ModelReportJob : ModelReportDates
    {
        public string CompanyId { get; set; }
        public string JobFieldId { get; set; }
    }
}
