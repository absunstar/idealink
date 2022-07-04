using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseBase
    {
        public string _id { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }

        public virtual void Map(object obj)
        {

        }
    }
}
