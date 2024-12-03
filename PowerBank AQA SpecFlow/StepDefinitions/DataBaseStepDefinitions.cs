using PowerBank_AQA_SpecFlow.Dto;
using PowerBank_AQA_SpecFlow.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_SpecFlow.StepDefinitions
{
    public class DataBaseStepDefinitions
    {
        private DbHelper dbHelper;
        public DataBaseStepDefinitions(DbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public CardDetailedInformation GetDetailedInfoAboutDebitCard(DataTable table)
        {
            return dbHelper.DataTableToCardDebitDetailedInformation(table);
        }

        public CardDetailedInformation GetDetailedInfoAboutCreditCard(DataTable table)
        {
            return dbHelper.DataTableToCardCreditDetailedInformation(table);
        }

        public CardDetailedInformation GetDetailedInfoAboutVirtualCard(DataTable table)
        {
            return dbHelper.DataTableToCardVirtualDetailedInformation(table);
        }

        public CardShortInformation GetShortInfoAboutDebitCard(DataTable table)
        {
            return dbHelper.DataTableToCardDebitShortInformation(table);
        }

        public CardShortInformation GetShortInfoAboutCreditCard(DataTable table)
        {
            return dbHelper.DataTableToCardCreditShortInformation(table);
        }

        public CardShortInformation GetShortInfoAboutVirtualCard(DataTable table)
        {
            return GetShortInfoAboutDebitCard(table);
        }
    }
}
