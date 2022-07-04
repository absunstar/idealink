using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Enum;

namespace Tadrebat.Entity.Mongo
{
    public class ContentData : MongoEntityNameBase
    {
        public EnumContentData Type { get; set; }
        public string Data { get; set; }
    }
}
