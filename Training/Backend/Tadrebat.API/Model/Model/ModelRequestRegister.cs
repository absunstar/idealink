using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelRequestRegister
    {
        public ModelRequestRegister()
        {
            data = new Dictionary<string, string>();
        }
        public string Type { get; set; }
        public string PartnerName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}
