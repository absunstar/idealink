using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class EntityTrainingCenter : MongoEntityBase
    {
        public EntityTrainingCenter()
        {
            //PartnerIds = new List<string>();
        }
        public string Name { get; set; }
        public string Phone { get; set; }
        //public List<string> PartnerIds { get; set; }
    }
}
