using Bogus.DataSets;
using PowerBank_AQA_DbTestingCore.DbClient;
using System.Data;

namespace PowerBank_AQA_SpecFlow.Support
{
    public class DbQueries
    {
        public static DataTable GetCardInformation(DbClient dbClient, string currencyCode)
        {
            return dbClient.SelectQuery("SELECT * FROM \"CardProduct\" cp WHERE \"Name\" = '" + currencyCode + "'");
        }
    }
}
