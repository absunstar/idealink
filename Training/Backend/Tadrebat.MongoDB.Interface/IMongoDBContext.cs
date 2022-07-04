using MongoDB.Driver;
using System;

namespace Tadrebat.MongoDB.Interface
{
    public interface IMongoDBContext
    {
        IMongoDatabase mongoDB { get; }
    }
}
