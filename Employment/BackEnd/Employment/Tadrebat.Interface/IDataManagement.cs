using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;

namespace Employment.Interface
{
    public interface IDataManagement
    {
        #region NGOType
        Task<NGOType> NGOTypeGetById(string Id);
        Task<bool> NGOTypeCreate(string Name);
        Task<bool> NGOTypeUpdate(string Id, string Name);
        Task<bool> NGOTypeDeActivate(string Id);
        Task<bool> NGOTypeActivate(string Id);
        Task<List<NGOType>> NGOTypeListActive();
        Task<MongoResultPaged<NGOType>> NGOTypeListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsNGOTypeExist(string Id);
        #endregion
    }
}
