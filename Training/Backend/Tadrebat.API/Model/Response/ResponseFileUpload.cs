using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseFileUpload
    {
        public ResponseFileUpload(string path)
        {
            this.Path = path;
        }
        public string Path { get; set; }
    }

}
    
