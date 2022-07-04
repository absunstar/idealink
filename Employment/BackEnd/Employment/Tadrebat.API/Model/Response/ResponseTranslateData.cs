using Employment.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseTranslate
    {
        public ResponseTranslate(EnumTranslateType type, List<ResponseTranslateData> data, string id)
        {
            Type = type;
            Data = data;
            Id = id;
        }
        public ResponseTranslate(EnumTranslateType type, List<ResponseTranslateData> data)
        {
            Type = type;
            Data = data;
        }
        public ResponseTranslate()
        {
            Data = new List<ResponseTranslateData>();
        }
        public EnumTranslateType Type { get; set; }
        public List<ResponseTranslateData> Data { get; set; }
        public string Id { get; set; }
    }
    public class ResponseTranslateData
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
    }

}
