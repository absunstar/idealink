using MongoDB.Driver;
using System;

namespace Employment.MongoDB.Interface
{
    public interface IMongoDBContext
    {
        IMongoDatabase mongoDB { get; }
    }
}
