using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class EntityPartner: MongoEntityBase
    {
        public EntityPartner()
        {
            MemberCanAccessIds = new List<string>();
            TrainingCenters = new List<EntityTrainingCenter>();
        }
        public string Name { get; set; }
        public string Phone { get; set; }

        public int MinHours { get; set; }
        public int MaxHours { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> MemberCanAccessIds { get; set; }
        public List<EntityTrainingCenter> TrainingCenters { get; set; }
    }
}
