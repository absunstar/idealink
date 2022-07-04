using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employment.API.Helpers.Constants;

namespace Employment.API.Model.Response
{
    public class ResponsePaged<T>
    {
        public ResponsePaged()
        {
            lstResult = new List<T>();
        }
        public ResponsePaged(long TotalCount, List<T> lst)
        {
            totalCount = TotalCount;
            lstResult = lst;
        }
        public long totalCount;
        public List<T> lstResult;
        public int pageSize = ConfigConstant.PageSize;
    }
}
