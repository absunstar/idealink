using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Model
{
    public class ModelTranslate
    {
        public ModelTranslate()
        {
            Data = new List<ModelTranslateData>();
        }
        public EnumTranslateType Type { get; set; }
        public List<ModelTranslateData> Data { get; set; }
        public string Id { get; set; }
    }
    public class ModelTranslateData
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
    }
}
