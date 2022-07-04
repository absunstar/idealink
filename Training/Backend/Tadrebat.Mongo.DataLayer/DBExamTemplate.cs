using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBExamTemplate : DBRepositoryMongo<ExamTemplate>, IDBExamTemplate
    {
        private static string _pDBCollectionName = "ExamTemplate";
        public DBExamTemplate(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<ExamTemplate>(_DBCollectionName);
        }
    }
}
