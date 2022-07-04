using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelResetPassword
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
