using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseUserProfile
    {
        public ResponseUserProfile()
        {
            MyCompanies = new List<ResponseSubItemActive>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int Type { get; set; }
        public bool IsEmployerLimitedCompanies { get; set; }
        public List<ResponseSubItemActive> MyCompanies { get; set; }
    }
}
