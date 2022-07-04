using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class LogoPartner : MongoEntityNameBase
    {
        public string WebsiteURL { get; set; }
        public string ImagePath { get; set; }
    }
}
