using MongoDB.Driver;
using Share.NoSql;
using System.Collections.Generic;

namespace Application.Service
{
    public class NoSQLConnection
    {
        private readonly IMongoCollection<LogDto> _logs;

        public NoSQLConnection(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _logs = database.GetCollection<LogDto>(settings.CollectionName);
        }

        public List<LogDto> Get() => _logs.Find(log => true).ToList();

        public LogDto Get(string id) => _logs.Find(log => log.Id == id).FirstOrDefault();

        public LogDto Create(LogDto log)
        {
            _logs.InsertOne(log);
            return log;
        }

        public void Update(string id, LogDto updatedGame) => _logs.ReplaceOne(log => log.Id == id, updatedGame);

        public void Delete(LogDto logForDeletion) => _logs.DeleteOne(log => log.Id == logForDeletion.Id);

        public void Delete(string id) => _logs.DeleteOne(log => log.Id == id);
    }
}
