using Bogus.DataSets;
using PowerBank_AQA_DbTestingCore.DbClient;
using System.Data;

namespace PowerBank_AQA_UITesting.Hooks
{
    public class DbQueries
    {
        public static DataTable GetDepositsInformation(DbClient dbClient, string currencyCode)
        {
            return dbClient.SelectQuery("SELECT * FROM public.\"DepositProducts\" WHERE \"CurrencyCode\" = '" + currencyCode + "'");
        }

        public static DataTable GetCardsInformation(DbClient dbClient, string currencyCode)
        {
            return dbClient.SelectQuery("SELECT * FROM \"CardProduct\" cp WHERE \"Type\" = '" + currencyCode + "' AND \"IsActive\" = true");
        }

        public static DataTable GetCardInformation(DbClient dbClient, string currencyCode)
        {
            return dbClient.SelectQuery("SELECT * FROM \"CardProduct\" cp WHERE \"Name\" = '" + currencyCode + "'");
        }

        public static DataRow GetLoansInformation(DbClient dbClient, string loansName)
        {
            return dbClient.SelectOneRow("SELECT \"Name\", \"InterestRate\", \"AmountMin\", \"AmountMax\", \"MinDurationMonths\", \"MaxDurationMonths\", \"IsRevocable\", \"IsGuarantee\" FROM public.\"CreditProduct\"  WHERE \"Name\" = '"+loansName+"'");
        }
    }
}
