using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Model
{
    public class ModelContentData
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public EnumContentData Type { get; set; }
        public string Data { get; set; }
    }
}
