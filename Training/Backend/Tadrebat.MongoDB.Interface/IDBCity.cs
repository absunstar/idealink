using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBCity : IRepositoryMongo<City>
    {
        Task UpdateName(string Id, string Name);
        Task<MongoResultPaged<City>> CityListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15);

        Task<bool> AreaCreate(string CityId, string Name);
        Task<bool> AreaUpdate(string CityId, string AreaId, string Name);
        Task<bool> AreaDeActivate(string CityId, string Id);
        Task<bool> AreaActivate(string CityId, string Id);
    }
}
