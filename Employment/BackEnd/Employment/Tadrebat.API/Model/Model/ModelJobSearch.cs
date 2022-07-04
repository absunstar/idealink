using Employment.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelJobSearch : ModelPaged
    {
        public string[] CompanyId { get; set; }
        public List<string> ExperienceId { get; set; }
        public List<int> GenderId { get; set; }
        public List<string> Qualificationid { get; set; }
        public List<string> IndustryId { get; set; }
        public List<string> JobFieldId { get; set; }
        public List<string> CountryId { get; set; }
        public List<string> CityId { get; set; }

        public string filterText { get; set; }
        public string filterTextValidation { get; set; }
        public List<string> filterTextSearch { get; set; }

    }
    public class ModelAdminJobSearch :ModelPaged
    {
        public string CompanyId { get; set; }
        public int StatusId { get; set; }
    }
}
