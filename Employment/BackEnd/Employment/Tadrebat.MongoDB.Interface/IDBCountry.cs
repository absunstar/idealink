using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;

namespace Employment.MongoDB.Interface
{
    public interface IDBCountry : IRepositoryMongo<Country>
    {
        Task<bool> SubCreate(string MainId, string Name);
        Task<bool> SubUpdate(string MainId, string SubId, string Name);
        Task<bool> SubActivate(string MainId, string Id);
        Task<bool> SubDeActivate(string MainId, string Id);
    }
}
