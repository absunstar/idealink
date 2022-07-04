using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelMyAssignedAccount
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AccountId { get; set; }
        [Required]
        public int Type{ get; set; }
    }
}
