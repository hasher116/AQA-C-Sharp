using System;

namespace PowerBank_AQA_UITesting.Dto
{
    public class OpenedDepositInformation
    {
        public string CurrencyCode { get; set; }
        public string OpenDate { get; set; }
        public string CloseDate { get; set; }
        public string Validity { get; set; }
        public int DepositAmount { get; set; }
        public int MinDepositAmount { get; set; }
        public double InterestRate { get; set; }
        public bool CurrentState { get; set; }
        public bool AutoRenewal { get; set; }
        public bool Capitalization { get; set; }
        public bool Revocable { get; set; }
    }
}
