using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelSubPartnerActiveSearch
    {
        public ModelSubPartnerActiveSearch()
        {
            PartnerIds = new List<string>();
        }
        public string query { get; set; }
        public List<string> PartnerIds { get; set; }
    }
}
