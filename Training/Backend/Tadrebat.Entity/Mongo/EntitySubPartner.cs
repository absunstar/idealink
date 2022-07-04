using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class EntitySubPartner : MongoEntityBase
    {
        public EntitySubPartner()
        {
            PartnerIds = new List<string>();
            TrainingCenterIds = new List<string>();
            MemberCanAccessIds = new List<string>();
        }
        public string Name { get; set; }
        public string Phone { get; set; }
        
        public List<string> PartnerIds { get; set; }
        public List<string> TrainingCenterIds { get; set; }
        public List<string> MemberCanAccessIds { get; set; }
    }

}
