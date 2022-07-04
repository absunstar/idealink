using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class MongoResultPaged<T>
    {
        public MongoResultPaged(long Count, List<T> lst, int PageSize)
        {
            totalCount = Count;
            lstResult = lst;
            pageSize = PageSize;
        }
        public long totalCount;
        public List<T> lstResult;
        public int pageSize = 15;
    }
}
