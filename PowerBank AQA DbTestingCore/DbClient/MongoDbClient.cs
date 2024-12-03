using PowerBank_AQA_DbTestingCore.Settings;
using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;

namespace PowerBank_AQA_DbTestingCore.DbClient
{
    public class MongoDbClient
    {
        private MongoSettings settings;
        private MongoClient client;
        private IMongoDatabase database;
        private IConfiguration configuration;

        public MongoDbClient(IConfiguration configuration, string sectionSettingsName = null)
        {
            this.configuration = configuration;
            sectionSettingsName ??= nameof(MongoSettings);
            var section = configuration.GetSection(sectionSettingsName);
            settings = section.Get<MongoSettings>()!;
        }
        public bool Create()
        {
            Log.Logger().LogInformation($"Попытка подключения к БД с параметрами: {settings.ConnectionString}");
            try
            {
                client = new MongoClient(settings.Url);
                database = client.GetDatabase(settings.DataBase);
                Log.Logger().LogInformation($"Соединение с БД успешно установлено");
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Возникла ошибка при попытке подключения к БД: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            IMongoCollection<T> collection;
            try
            {
                collection = database.GetCollection<T>(collectionName);
                Log.Logger().LogInformation($"Коллекция получена");
                return collection;
            }

            catch (Exception ex)
            {
                Log.Logger().LogError($"Коллекция не получена: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public List<T> GetDocuments<T>(IMongoCollection<T> collection)
        {
            List<T> documents;
            try
            {
                documents = collection.Find(new BsonDocument()).ToList();
                Log.Logger().LogInformation($"Документы получены");
                return documents;
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Документы не получены: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}