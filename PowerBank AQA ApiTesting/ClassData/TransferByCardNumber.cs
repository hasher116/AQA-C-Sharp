using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class TransferByCardNumber
    {
        public string SenderCardNumber { get; set; }
        public string CardVerification { get; set; }
        public string RecipientCardNumber { get; set; }
        public string CurrencyCode { get; set; }
        public string AmountTransfer { get; set; }
        public string AmountCommission { get; set; }
        public string AmountWithdrawal { get; set; }
    }
}
