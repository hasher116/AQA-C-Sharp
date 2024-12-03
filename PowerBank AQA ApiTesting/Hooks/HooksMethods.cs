using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_DbTestingCore.DbClient;
using System.Data;

namespace PowerBank_AQA_ApiTesting.Hooks
{
    public static class HooksMethods
    {
        public static bool KeepTrying(Func<bool> operation, int timeout = 3)
        {
            bool success = false;

            int elapsed = 0;
            while ((!success) && (elapsed < timeout))
            {
                Thread.Sleep(1000);
                elapsed += 1000;
                success = operation();
            }
            return success;
        }

        public static bool Operation(DbClient dbClient, string depositProductId, string clientId)
        { 
            var dataRow = dbClient.SelectOneRow("SELECT \"Acc\".\"AccountNumber\", \"Acc\".\"CurrentBalance\" FROM public.\"Accounts\" AS \"Acc\" JOIN public.\"Agreements\" AS \"Ag\" ON \"Acc\".\"AgreementId\" = \"Ag\".\"Id\" WHERE \"Ag\".\"ClientId\" = '" + clientId + "' AND \"Ag\".\"DepositProductId\" = '" + depositProductId + "';");

            var mapper = MyMapper.Balance.CreateMapper();
            var mapperHelperAfterOperation = mapper.Map<DataRow, DbHelperBalance>(dataRow);
            var depositBalanceDbAfterOperation = mapperHelperAfterOperation.CurrentBalance;
            if (depositBalanceDbAfterOperation == 0)
              return true;
            else
                return false;
        }
    }
}
