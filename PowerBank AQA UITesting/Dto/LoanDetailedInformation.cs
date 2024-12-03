using System;

namespace PowerBank_AQA_UITesting.Dto
{
    public class LoanDetailedInformation
    {
        public string Name { get; set; }
        public int InterestRate { get; set; }
        public int AmountMin { get; set; }
        public int AmountMax { get; set; }
        public int MinDurationMonths { get; set; }
        public int MaxDurationMonths { get; set; }
        public bool IsGuarantee { get; set; }
        public bool IsRevocable { get; set; }
    }
}
