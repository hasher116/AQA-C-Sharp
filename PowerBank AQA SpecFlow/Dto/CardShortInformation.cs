using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_SpecFlow.Dto
{
    public class CardShortInformation
    {
        public string CardName { get; set; }
        public string CardType { get; set; }
        public string CardServicePrice { get; set; }
        public string CardNotificationPrice { get; set; }
        public string CardPaymentSystem { get; set; }
        public string CardCashBack { get; set; }
        public string CardShortDescription { get; set; }
        public string CardInterestRate { get; set; }
        public string CardInterestFreeDays { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CardShortInformation information &&
                   CardName == information.CardName &&
                   CardType == information.CardType &&
                   CardServicePrice == information.CardServicePrice &&
                   CardNotificationPrice == information.CardNotificationPrice &&
                   CardPaymentSystem == information.CardPaymentSystem &&
                   CardCashBack == information.CardCashBack &&
                   CardShortDescription == information.CardShortDescription &&
                   CardInterestRate == information.CardInterestRate &&
                   CardInterestFreeDays == information.CardInterestFreeDays;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(CardName);
            hash.Add(CardType);
            hash.Add(CardServicePrice);
            hash.Add(CardNotificationPrice);
            hash.Add(CardPaymentSystem);
            hash.Add(CardCashBack);
            hash.Add(CardShortDescription);
            hash.Add(CardInterestRate);
            hash.Add(CardInterestFreeDays);
            return hash.ToHashCode();
        }
    }
}