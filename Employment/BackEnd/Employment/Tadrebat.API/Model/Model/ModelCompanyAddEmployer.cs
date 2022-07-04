using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelCompanyAddEmployer
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CompanyId { get; set; }
    }
}
