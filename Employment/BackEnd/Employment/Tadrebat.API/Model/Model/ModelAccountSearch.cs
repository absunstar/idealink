﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelAccountSearch : ModelPaged
    {
        public int filterType { get; set; }
    }
}
