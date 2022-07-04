using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelEntityPartner
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int MinHours { get; set; }
        public int MaxHours { get; set; }
    }
}
