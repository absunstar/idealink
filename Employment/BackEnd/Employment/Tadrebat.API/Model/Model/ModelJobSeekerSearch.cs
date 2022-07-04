using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelJobSeekerSearch : ModelPaged
    {
        public List<string> ExperienceId { get; set; }
        public List<string> GenderId { get; set; }
        public List<string> Qualificationid { get; set; }
        public List<string> LanguageId { get; set; }
        public string CountryId { get; set; }
        public string CityId { get; set; }
    }
}
