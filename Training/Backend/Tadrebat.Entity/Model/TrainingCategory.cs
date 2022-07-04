using System;
using System.Collections.Generic;

namespace Tadrebat.Model.Entity
{
    public partial class TrainingCategory : BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
