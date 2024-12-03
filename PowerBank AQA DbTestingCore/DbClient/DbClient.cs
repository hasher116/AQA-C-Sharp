using System.Data;
using PowerBank_AQA_DbTestingCore.DbHelper;
using PowerBank_AQA_DbTestingCore.Settings;
using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;


namespace PowerBank_AQA_DbTestingCore.DbClient
{
    public class DbClient : IDbClient
    {
        private PostgresSettings settings;

        private IConfiguration configuration { get; set; }

        private NpgsqlConnection connection;

        public DbClient(IConfiguration configuration, string sectionSettingsName = null)
        {
            this.configuration = configuration;
            sectionSettingsName ??= nameof(PostgresSettings);
            var section = configuration.GetSection(sectionSettingsName);
            settings = section.Get<PostgresSettings>()!;
        }
        public bool Create()
        {
            Log.Logger().LogInformation($"Попытка подключения к БД с параметрами: {settings.ConnectionString}");
            try
            {
                connection = new NpgsqlConnection(settings.ConnectionString);
                connection.Open();
                Log.Logger().LogInformation($"Соединение с БД успешно установлено");
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Возникла ошибка при попытке подключения к БД: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public DataTable SelectQuery(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                var command = new NpgsqlCommand(query, connection);
                Log.Logger().LogInformation($"Выполнение SQL запроса: {query}");

                DataSet ds = new();
                DataTable dt = new();

                try
                {
                    command.Prepare();
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    adapter.Fill(ds);
                    dt = ds.Tables[0];
                    Log.Logger().LogInformation($"Запрос вернул: {Environment.NewLine}{(dt != null ? dt.CreateMessage() : "пустой")}");
                    return dt;

                }
                catch (Exception ex)
                {
                    Log.Logger().LogError($"При выполнении SQL запроса произошла ошибка {ex.Message}");
                    throw new NpgsqlException("Ошибка при выполнении SQL запроса, подробности - в логах");
                }
            }

            else
            {
                throw new ArgumentException("В качестве запроса передана пустая строка");
            }
        }

        public DataRow SelectOneRow(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                var command = new NpgsqlCommand(query, connection);
                Log.Logger().LogInformation($"Выполнение SQL запроса: {query}");

                DataSet ds = new();
                DataTable dt = new();

                try
                {
                    command.Prepare();
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    adapter.Fill(ds);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count != 1)
                    {
                        Log.Logger().LogError($"SQL запрос вернул {dt.Rows.Count} записей");
                        throw new NpgsqlException("Количество записей в запросе отличается от 1");
                    }
                    Log.Logger().LogInformation($"Запрос вернул: {Environment.NewLine}{(dt != null ? dt.Rows[0].CreateMessage() : "пустой")}");
                    return dt.Rows[0];

                }
                catch (Exception ex)
                {
                    Log.Logger().LogError($"При выполнении SQL запроса произошла ошибка {ex.Message}");
                    throw new NpgsqlException("Ошибка при выполнении SQL запроса, подробности - в логах");
                }
            }
            else
            {
                throw new ArgumentException("В качестве запроса передана пустая строка");
            }

        }

        public void NonQuery(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                var command = new NpgsqlCommand(query, connection);
                Log.Logger().LogInformation($"Выполнение SQL запроса: {query}");

                try
                {
                    var isSuccess = command.ExecuteNonQuery();
                    Log.Logger().LogInformation($"Запрос вернул: {Environment.NewLine}{isSuccess}");
                }
                catch (Exception ex)
                {
                    Log.Logger().LogError($"При выполнении SQL запроса произошла ошибка {ex.Message}");
                    throw new NpgsqlException("Ошибка при выполнении SQL запроса, подробности - в логах");
                }
            }
            else
            {
                throw new ArgumentException("В качестве запроса передана пустая строка");
            }
        }

        public string SelectOneCellAsString(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                var command = new NpgsqlCommand(query, connection);
                Log.Logger().LogInformation($"Выполнение SQL запроса: {query}");

                try
                {
                    var result = command.ExecuteScalar().ToString();
                    Log.Logger().LogInformation($"Запрос вернул: {Environment.NewLine}{(result != null ? result : "пустой")}");
                    return result;

                }
                catch (Exception ex)
                {
                    Log.Logger().LogError($"При выполнении SQL запроса произошла ошибка {ex.Message}");
                    throw new NpgsqlException("Ошибка при выполнении SQL запроса, подробности - в логах");
                }
            }
            else
            {
                throw new ArgumentException("В качестве запроса передана пустая строка");
            }
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}