using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class Country : MongoEntityNameBase
    {
        public Country()
        {
            subItems = new List<City>();
        }
        public List<City> subItems { get; set; }
    }
    public class City : MongoEntityNameBase
    {
    }
}
