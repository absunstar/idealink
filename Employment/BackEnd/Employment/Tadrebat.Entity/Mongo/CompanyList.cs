using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class CompanyList
    {
        public bool IsActive { get; set; } = true;
        public string Status { get; set; } = "Approved";
        public string Company { get; set; }
    }
}
