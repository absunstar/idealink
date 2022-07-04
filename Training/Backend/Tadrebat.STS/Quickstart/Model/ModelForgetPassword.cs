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
        [RegularExpression(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$",ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
    }
}
