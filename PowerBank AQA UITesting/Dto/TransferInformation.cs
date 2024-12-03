using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITesting.Dto
{
    public class TransferInformation
    {
        public string CardNumberSender { get; set; }
        public string CardNumberRecipient { get; set; }
        public string Amount { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TransferInformation information &&
                   CardNumberSender == information.CardNumberSender &&
                   CardNumberRecipient == information.CardNumberRecipient &&
                   Amount == information.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CardNumberSender, CardNumberRecipient, Amount);
        }
    }
}
