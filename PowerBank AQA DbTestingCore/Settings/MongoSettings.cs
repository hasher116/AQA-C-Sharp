using System;

namespace PowerBank_AQA_DbTestingCore.Settings
{
    public class MongoSettings
    {
        public string? Url { get; set; }
        public string? DataBase { get; set; }
        public string? Collection { get; set; }

        public string ConnectionString
        {
            get { return $"Url={Url}; DataBase={DataBase}; Collections={Collection}"; }
        }
    }
}