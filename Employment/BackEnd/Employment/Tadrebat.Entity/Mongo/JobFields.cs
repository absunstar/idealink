using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class JobFields : MongoEntityNameBase
    {
        public JobFields()
        {
            subItems = new List<JobSubFields>();
        }
        public List<JobSubFields> subItems { get; set; }
    }
    public class JobSubFields : MongoEntityNameBase
    {
    }
}
