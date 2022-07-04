using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class City : MongoEntityBase
    {
        public City()
        {
            areas = new List<Area>();
        }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public List<Area> areas { get; set; }
    }
}
