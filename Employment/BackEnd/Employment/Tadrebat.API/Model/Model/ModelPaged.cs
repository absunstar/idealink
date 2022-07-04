using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employment.API.Helpers.Constants;

namespace Employment.API.Model.Model
{
    public class ModelPaged
    {
        public string filterText = "";
        public int CurrentPage = 1;
        public int PageSize = ConfigConstant.PageSize; 
    }
}
