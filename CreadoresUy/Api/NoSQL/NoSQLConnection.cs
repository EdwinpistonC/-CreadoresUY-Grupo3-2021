using MongoDB.Driver;
using Share.NoSql;
using System.Collections.Generic;

namespace Api.NoSQL
{
    public class NoSQLConnection
    {
        private readonly IMongoCollection<LogDto> _games;

        public NoSQLConnection(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _games = database.GetCollection<LogDto>(settings.GamesCollectionName);
        }

        public List<LogDto> Get() => _games.Find(game => true).ToList();

        public LogDto Get(string id) => _games.Find(game => game.Id == id).FirstOrDefault();

        public LogDto Create(LogDto game)
        {
            _games.InsertOne(game);
            return game;
        }

        public void Update(string id, LogDto updatedGame) => _games.ReplaceOne(game => game.Id == id, updatedGame);

        public void Delete(LogDto gameForDeletion) => _games.DeleteOne(game => game.Id == gameForDeletion.Id);

        public void Delete(string id) => _games.DeleteOne(game => game.Id == id);
    }
}
