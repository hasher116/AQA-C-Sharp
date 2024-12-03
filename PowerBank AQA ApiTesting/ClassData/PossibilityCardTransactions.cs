using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class PossibilityCardTransactions
    {
        public string SenderCardNumber { get; set; }
        public string CardVerification { get; set; }
        public string RecipientCardNumber { get; set; }
        public string CurrencyCode { get; set; }
        public decimal AmountTransfer { get; set; }
        public decimal AmountCommission { get; set; }
        public decimal AmountWithdrawal { get; set; }
        public string TransactionComment { get; set; }
    }
}
