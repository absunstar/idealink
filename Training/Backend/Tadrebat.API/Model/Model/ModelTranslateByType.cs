using iText.IO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Model
{
    public class ModelTranslateByType
    {
        public EnumTranslateType Type { get; set; }
        public string Id { get; set; }
    }
}
