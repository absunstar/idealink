using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseEntityTrainingCenter
    {
        public ResponseEntityTrainingCenter()
        {
            //Partners = new List<ResponseEntityPartner>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        //public List<ResponseEntityPartner> Partners{get;set; }
    }
}
