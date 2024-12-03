using PowerBank_AQA_UITesting.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITesting.Dto
{
    public class CardDetailedInformation : CardShortInformation
    {
        public string CardCashMaxDay { get; set; }
        public string CardCashMaxMonth { get; set; }
        public string CardAmountCreditMax { get; set; }
        public string CardCashWithdrawalFee { get; set; }
        public bool CardTariff { get; set; } = true;
        public bool CardService { get; set; } = true;

        public override bool Equals(object? obj)
        {
            return obj is CardDetailedInformation information &&
                   base.Equals(obj) &&
                   CardName == information.CardName &&
                   CardType == information.CardType &&
                   CardServicePrice == information.CardServicePrice &&
                   CardNotificationPrice == information.CardNotificationPrice &&
                   CardPaymentSystem == information.CardPaymentSystem &&
                   CardCashBack == information.CardCashBack &&
                   CardShortDescription == information.CardShortDescription &&
                   CardInterestRate == information.CardInterestRate &&
                   CardInterestFreeDays == information.CardInterestFreeDays &&
                   CardCashMaxDay == information.CardCashMaxDay &&
                   CardCashMaxMonth == information.CardCashMaxMonth &&
                   CardAmountCreditMax == information.CardAmountCreditMax &&
                   CardCashWithdrawalFee == information.CardCashWithdrawalFee &&
                   CardTariff == information.CardTariff &&
                   CardService == information.CardService;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(CardName);
            hash.Add(CardType);
            hash.Add(CardServicePrice);
            hash.Add(CardNotificationPrice);
            hash.Add(CardPaymentSystem);
            hash.Add(CardCashBack);
            hash.Add(CardShortDescription);
            hash.Add(CardInterestRate);
            hash.Add(CardInterestFreeDays);
            hash.Add(CardCashMaxDay);
            hash.Add(CardCashMaxMonth);
            hash.Add(CardAmountCreditMax);
            hash.Add(CardCashWithdrawalFee);
            hash.Add(CardTariff);
            hash.Add(CardService);
            return hash.ToHashCode();
        }
    }
}
