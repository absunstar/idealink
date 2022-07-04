using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
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

    public class MongoResultPaged_new<T>
    {
        public MongoResultPaged_new(long Count, IEnumerable<T> lst, int PageSize)
        {
            totalCount = Count;
            lstResult = lst;
            pageSize = PageSize;
        }
        public long totalCount;
        public IEnumerable<T> lstResult;
        public int pageSize = 15;
    }
}
