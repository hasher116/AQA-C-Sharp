﻿using System.Data;
using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_SpecFlow.Dto;
using System.Collections.Generic;
using System;

namespace PowerBank_AQA_SpecFlow.Support
{
    public class DbHelper
    {
        public static T SwitchCardType<T>(T card) where T : CardShortInformation
        {
            switch (card.CardType)
            {
                case "debet":
                    {
                        card.CardType = "Дебетовая карта";
                        break;
                    }
                case "credit":
                    {
                        card.CardType = "Кредитная карта";
                        break;
                    }
                case "virtual":
                    {
                        card.CardType = "Виртуальная карта";
                        break;
                    }
            }
            return card;
        }

        public bool IsDataNameExistsInColumn(DataTable dataTable, string columnName, string dataName)
        {
            int index = 0;
            DataColumnCollection columns = dataTable.Columns;

            if (columns.Contains(columnName))
                index = columns.IndexOf(columnName);

            var array = dataTable.Rows.Cast<DataRow>().Select(x => x.ItemArray[index]);

            if (array.Contains(dataName))
            {
                Log.Logger().LogInformation($"{dataName} +  from table");
                return true;
            }

            else return false;
        }

        public CardDetailedInformation DataTableToCardDebitDetailedInformation(DataTable dataTable)
        {
            CardDetailedInformation card = new CardDetailedInformation();
            DataRow dataRow = dataTable.Rows[0];
            card.CardName = dataRow.Field<string>("Name");
            card.CardType = dataRow.Field<string>("Type");
            card.CardType = SwitchCardType(card).CardType;
            card.CardCashBack = Convert.ToInt32(dataRow.Field<decimal>("CashbackMax")).ToString();
            card.CardServicePrice = Convert.ToInt32(dataRow.Field<decimal>("ServicePrice")).ToString();
            card.CardCashMaxDay = Convert.ToInt32(dataRow.Field<decimal>("CashMaxDay")).ToString();
            card.CardCashMaxMonth = Convert.ToInt32(dataRow.Field<decimal>("CashMaxMonth")).ToString();
            card.CardNotificationPrice = Convert.ToInt32(dataRow.Field<decimal>("NotificationPrice")).ToString();
            card.CardPaymentSystem = dataRow.Field<string>("PaymentSystem");
            return card;
        }

        public CardDetailedInformation DataTableToCardCreditDetailedInformation(DataTable dataTable)
        {
            CardDetailedInformation card = new CardDetailedInformation();
            DataRow dataRow = dataTable.Rows[0];
            card.CardName = dataRow.Field<string>("Name");
            card.CardType = dataRow.Field<string>("Type");
            card.CardType = SwitchCardType(card).CardType;
            card.CardPaymentSystem = dataRow.Field<string>("PaymentSystem");
            card.CardInterestRate = Convert.ToInt32(dataRow.Field<decimal>("InterestRate")).ToString();
            card.CardInterestFreeDays = dataRow.Field<int>("InterestFreeDays").ToString();
            card.CardAmountCreditMax = Convert.ToInt32(dataRow.Field<decimal>("AmountCreditMax")).ToString();
            card.CardServicePrice = Convert.ToInt32(dataRow.Field<decimal>("ServicePrice")).ToString();
            card.CardCashMaxDay = Convert.ToInt32(dataRow.Field<decimal>("CashMaxDay")).ToString();
            card.CardNotificationPrice = Convert.ToInt32(dataRow.Field<decimal>("NotificationPrice")).ToString();
            card.CardCashWithdrawalFee = Convert.ToInt32(dataRow.Field<decimal>("CashWithdrawalFee")).ToString();
            return card;
        }

        public CardDetailedInformation DataTableToCardVirtualDetailedInformation(DataTable dataTable)
        {
            CardDetailedInformation card = new CardDetailedInformation();
            DataRow dataRow = dataTable.Rows[0];
            card.CardName = dataRow.Field<string>("Name");
            card.CardType = dataRow.Field<string>("Type");
            card.CardType = SwitchCardType(card).CardType;
            card.CardPaymentSystem = dataRow.Field<string>("PaymentSystem");
            card.CardCashBack = Convert.ToInt32(dataRow.Field<decimal>("CashbackMax")).ToString();
            card.CardServicePrice = Convert.ToInt32(dataRow.Field<decimal>("ServicePrice")).ToString();
            card.CardNotificationPrice = Convert.ToInt32(dataRow.Field<decimal>("NotificationPrice")).ToString();
            return card;
        }

        public CardShortInformation DataTableToCardDebitShortInformation(DataTable dataTable)
        {
            CardShortInformation card = new CardShortInformation();
            DataRow dataRow = dataTable.Rows[0];
            card.CardName = dataRow.Field<string>("Name");
            card.CardType = dataRow.Field<string>("Type");
            card.CardType = SwitchCardType(card).CardType;
            card.CardShortDescription = dataRow.Field<string>("ShortDescription");
            card.CardCashBack = Convert.ToInt32(dataRow.Field<decimal>("CashBackMax")).ToString();
            card.CardServicePrice = Convert.ToInt32(dataRow.Field<decimal>("ServicePrice")).ToString();
            return card;
        }

        public CardShortInformation DataTableToCardCreditShortInformation(DataTable dataTable)
        {
            CardShortInformation card = new CardShortInformation();
            DataRow dataRow = dataTable.Rows[0];
            card.CardName = dataRow.Field<string>("Name");
            card.CardType = dataRow.Field<string>("Type");
            card.CardType = SwitchCardType(card).CardType;
            card.CardShortDescription = dataRow.Field<string>("ShortDescription");
            card.CardInterestRate = Convert.ToInt32(dataRow.Field<decimal>("InterestRate")).ToString();
            card.CardInterestFreeDays = dataRow.Field<int>("InterestFreeDays").ToString();
            card.CardServicePrice = Convert.ToInt32(dataRow.Field<decimal>("ServicePrice")).ToString();
            return card;
        }
    }
}