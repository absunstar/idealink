using Employment.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelFileUpload
    {
        public string FileName { get; set; }
        public EnumFileType type { get; set; }
    }
}
