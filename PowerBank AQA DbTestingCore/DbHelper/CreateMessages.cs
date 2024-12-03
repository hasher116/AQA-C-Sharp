using PowerBank_AQA_DbTestingCore.DbExtensions;
using PowerBank_AQA_DbTestingCore.Infrastructure;
using System.Data;


namespace PowerBank_AQA_DbTestingCore.DbHelper
{
    public static class CreateMessages
    {
        public static string CreateMessage(this DataTable dataTable)
        {
            var (str, isMoreMaxRows) = dataTable.ConvertToString();

            if (isMoreMaxRows)
            {
                str += $"...{Environment.NewLine} Таблица содержит больше {Constants.MAX_ROWS} строк";
            }

            return str;
        }

        public static string CreateMessage(this DataRow dataRow)
        {
            return dataRow.ConvertToString();
        }
    }
}
