using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Model
{
    public class ModelConfigForm
    {
        public EnumConfigForm FormType { get; set; }
        public List<ModelFieldConfig> Form { get; set; }
    }
    public class ModelFieldConfig
    {
        public int order { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string inputType { get; set; }
        public string type { get; set; }
        public List<ModelOptions> options { get; set; }
        public bool IsRequired { get; set; }
    }
    public class ModelOptions
    {
        public string option { get; set; }
    }
    public class Validator
    {
        public string name { get; set; }
        public bool enabled { get; set; }
    }
}
