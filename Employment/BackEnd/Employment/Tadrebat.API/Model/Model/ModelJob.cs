using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelJob: ModelPaged
    {
        public string Name { get; set; }
        public string _id { get; set; }
        public int Gender { get; set; }
        public int Type { get; set; }
        public string Remuneration { get; set; }
        public string CompanyId { get; set; }
        public string Description { get; set; }
        public string Skills { get; set; }
        public string Benefits { get; set; }
        public DateTime Deadline { get; set; }
        public string JobFieldId { get; set; }
        public string JobSubFieldId { get; set; }
        public string ExperienceId { get; set; }
        public string IndustryId { get; set; }
        public string QualificationId { get; set; }
        public string CountryId { get; set; }
        public string CityId { get; set; }
        public string Address { get; set; }
    }
}
