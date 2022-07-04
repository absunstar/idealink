using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelExamTemplate 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsDefault { get; set; }
        public int Easy { get; set; }
        public int Medium { get; set; }
        public int Hard { get; set; }
    }
}
