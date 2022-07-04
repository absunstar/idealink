using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseCity
    {
        public ResponseCity()
        {
            Areas = new List<ResponseArea>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public List<ResponseArea> Areas { get; set; }
    }
}
