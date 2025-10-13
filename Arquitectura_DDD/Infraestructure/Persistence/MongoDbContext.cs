using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace Arquitectura_DDD.Infraestructure.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            // Usar "Arquitectura" como nombre de la base de datos
            _database = client.GetDatabase("Arquitectura");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        private static string? ExtractDatabaseName(string connectionString)
        {
            // Extraer el nombre de la base de datos de la connection string
            var uri = new Uri(connectionString);
            var databaseName = uri.AbsolutePath.TrimStart('/');
            return string.IsNullOrEmpty(databaseName) ? null : databaseName;
        }
    }
}
