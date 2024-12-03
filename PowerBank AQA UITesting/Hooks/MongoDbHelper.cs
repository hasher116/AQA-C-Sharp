using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITesting.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITesting.Hooks
{
    public class MongoDbHelper
    {
        public List<ExchangeRate> ConvertBsonToListExchangeRate(List<BsonDocument> documents)
        {
            List<ExchangeRate> list = new List<ExchangeRate>();
            foreach (var document in documents)
            {
                ExchangeRate exchangeRate = new ExchangeRate();
                exchangeRate._id = document.GetElement("_id").Value.AsString;
                exchangeRate.LastUpdateTime = document.GetElement("LastUpdateTime").Value.AsDateTime;
                exchangeRate.CurrencyName = document.GetElement("CurrencyName").Value.AsString;
                exchangeRate.CurrencyCode = document.GetElement("CurrencyCode").Value.AsString;
                exchangeRate.Unit = document.GetElement("Unit").Value.AsInt32;
                exchangeRate.BuyingRate = Math.Round(document.GetElement("BuyingRate").Value.AsDouble / exchangeRate.Unit, 2);
                exchangeRate.SellingRate = Math.Round(document.GetElement("SellingRate").Value.AsDouble / exchangeRate.Unit, 2);
                list.Add(exchangeRate);
            }
            return list;
        }

        public LoanDetailedInformation ConvertBsonToListLoans(BsonDocument document)
        {
            LoanDetailedInformation info = new LoanDetailedInformation();
            info.Name = document.GetElement("name").Value.AsString;
            info.InterestRate = Convert.ToInt32(TrimString.GetNumbersInString(document.GetElement("interestRate").Value.AsString));
            info.AmountMin = document.GetElement("amountMin").Value.AsInt32;
            info.AmountMax = document.GetElement("amountMax").Value.AsInt32;
            info.MinDurationMonths = document.GetElement("minDurationMonths").Value.AsInt32;
            info.MaxDurationMonths = document.GetElement("maxDurationMonths").Value.AsInt32;
            info.IsGuarantee = document.GetElement("isGuarantee").Value.AsBoolean;
            info.IsRevocable = document.GetElement("isRevocable").Value.AsBoolean;

            return info;
        }
    }
}