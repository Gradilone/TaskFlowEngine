using MongoDB.Driver;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDbSettings:ConnectionString").Value);
            _database = client.GetDatabase(configuration.GetSection("MongoDbSettings:DatabaseName").Value);
        }
        public IMongoCollection<Tarefa> Tarefas => _database.GetCollection<Tarefa>("Tarefas");
    }
}
