using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Quickstart.UI
{
    public class ModelForgetPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
