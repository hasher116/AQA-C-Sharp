using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class CardTransactionHistory
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string AccountNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TransactionType { get; set; }
        public int TransactionMarker { get; set; }
    }
}
