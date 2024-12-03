using Bogus.DataSets;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_DbTestingCore.DbClient;
using System.Collections.Generic;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace PowerBank_AQA_ApiTesting.Hooks
{
    public static class DbQueries
    {
        public static void DeleteUser(RegistrationNewUser user, DbClient dbClient)
        {
            dbClient.NonQuery("DELETE FROM public.\"PassportData\" WHERE \"IdentificationPassportNumber\" = '" + user.PassportNumber + "';");
        }

        public static DataRow CheckAccountNumberAndDepositBalance(DbClient dbClient, string depositProductId, string clientId)
        {
            return dbClient.SelectOneRow("SELECT \"Acc\".\"AccountNumber\", \"Acc\".\"CurrentBalance\" FROM public.\"Accounts\" AS \"Acc\" JOIN public.\"Agreements\" AS \"Ag\" ON \"Acc\".\"AgreementId\" = \"Ag\".\"Id\" WHERE \"Ag\".\"ClientId\" = '" + clientId + "' AND \"Ag\".\"DepositProductId\" = '" + depositProductId + "';");
        }

        public static string GetCardNumber(DbClient dbClient, string usersName, string clientId)
        {
            var table = dbClient.SelectQuery("SELECT \"C\".\"CardNumber\" FROM public.\"Card\" AS \"C\" JOIN public.\"Account\" AS \"Acc\" ON \"C\".\"AccountId\" = \"Acc\".\"Id\" JOIN public.\"Agreement\" AS \"Agr\" ON \"Acc\".\"AgreementId\" = \"Agr\".\"Id\" WHERE \"Agr\".\"ClientId\" = '" + clientId + "' AND \"C\".\"HolderName\" = '" + usersName + "' AND \"C\".\"IsActive\" = 'true'");
            if (table != null)
            {
                return table.Rows[0][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetAccountNumber(DbClient dbClient, string depositProductId, string clientId)
        {
            return dbClient.SelectOneCellAsString("SELECT \"Acc\".\"AccountNumber\", \"Acc\".\"CurrentBalance\" FROM public.\"Accounts\" AS \"Acc\" JOIN public.\"Agreements\" AS \"Ag\" ON \"Acc\".\"AgreementId\" = \"Ag\".\"Id\" WHERE \"Ag\".\"ClientId\" = '" + clientId + "' AND \"Ag\".\"DepositProductId\" = '" + depositProductId + "';");
        }

        public static DataRow CheckTransactions(DbClient dbClient, string accountNumber)
        {
            return dbClient.SelectOneRow("SELECT * FROM public.\"DepositTransactions\" WHERE \"AccountNumber\" = '" + accountNumber + "'");
        }

        public static string GetClientId(DbClient dbClient, string usersEmail)
        {
            return dbClient.SelectOneCellAsString("SELECT  \"ClientId\"  FROM \"UserProfiles\" WHERE \"Email\"= '" + usersEmail + "'");
        }

        public static string GetDepositProductId(DbClient dbClient, string depositName)
        {
            return dbClient.SelectOneCellAsString("SELECT \"Id\" FROM public.\"DepositProducts\" WHERE \"Name\"='" + depositName + "'");
        }

        public static string GetDepositAgreementId(DbClient dbClient, string clientId, string depositProductId)
        {
            return dbClient.SelectOneCellAsString("SELECT \"Id\" FROM public.\"Agreements\" WHERE \"ClientId\"='" + clientId + "' AND \"DepositProductId\"= '" + depositProductId + "'");
        }

        public static DataTable MoneyTransfer(DbClient dbClient, string accountNumberForTransactions)
        {
            return dbClient.SelectQuery("SELECT * FROM public.\"CardTransactions\" WHERE \"AccountNumber\" = '" + accountNumberForTransactions + "'");
        }

        public static void DeleteTransactions(DbClient dbClient, string accountNumber)
        {
            dbClient.NonQuery("Delete FROM public.\"DepositTransactions\" WHERE \"AccountNumber\" = '" + accountNumber + "'");
        }

        public static void CloseDeposit(DbClient dbClient, string accountNumber)
        {
            dbClient.NonQuery("UPDATE public.\"Accounts\" SET \"IsOpened\"=false, \"InitialCloseDate\" = current_date::timestamp WHERE \"AccountNumber\" = '" + accountNumber + "' ");
        }

        public static int CheckAmountMinTransaction(DbClient dbClient, string nameDepositProduct)
        {
            var minAmount = dbClient.SelectOneCellAsString("SELECT \"AmountMin\" FROM public.\"DepositProducts\" WHERE \"Name\" = '" + nameDepositProduct + "'");
            //var separator = minAmount.IndexOf(',');
            //minAmount = minAmount.Substring(0, separator);
            minAmount = minAmount.Substring(0, 4);
            return int.Parse(minAmount);
        }
        public static void BlockCard(DbClient dbClient, string cardNumber)
        {
            dbClient.NonQuery("UPDATE public.\"Card\"SET \"IsActive\" = false  WHERE \"CardNumber\" = '" + cardNumber + "'");
        }

        public static void UnblockCard(DbClient dbClient, string cardNumber)
        {
            dbClient.NonQuery("UPDATE public.\"Card\"SET \"IsActive\" = true  WHERE \"CardNumber\" = '" + cardNumber + "'");
        }

        public static bool IsUserBlockedCard(DbClient dbClient, string cardNumber)
        {
            return dbClient.SelectOneRow("SELECT \"IsUserBlocked\" FROM \"Card\" c  WHERE \"CardNumber\" = '" + cardNumber + "'").Field<bool>(0);
        }

        public static string GetEmail(DbClient dbClient, string userPhoneNumber)
        {
            return dbClient.SelectOneRow("SELECT \"Up\".\"Email\" FROM public.\"UserProfiles\" AS \"Up\" JOIN public.\"Clients\" AS \"Cl\" ON \"Up\".\"ClientId\" = \"Cl\".\"Id\" WHERE \"Cl\".\"MobilePhone\" = '" + userPhoneNumber + "'").Field<string>(0);
        }

        public static bool GetNotification(DbClient dbClient)
        {
            return dbClient.SelectOneRow("SELECT \"up\".\"EmailSubscription\" FROM \"UserProfiles\" up WHERE \"ClientId\" = 'e8923514-b59c-11ec-b909-0242ac120002'").Field<bool>(0);
        }
    }
}
