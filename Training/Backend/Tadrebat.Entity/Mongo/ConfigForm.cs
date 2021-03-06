using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Enum;

namespace Tadrebat.Entity.Mongo
{
    public class ConfigForm : MongoEntityNameBase
    {
        public EnumConfigForm FormType { get; set; }
        public List<FieldConfig> Form { get; set; }
    }
    public class FieldConfig
    {
        public int order { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public string inputType { get; set; }
        public string type { get; set; }
        public List<Options> options { get; set; }
        public bool IsRequired { get; set; }
    }
    public class Options
    {
        public string option { get; set; }
    }
}
