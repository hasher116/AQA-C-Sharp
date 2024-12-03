using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITesting.Dto
{
    public class ExchangeRate
    {
        public string _id { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public double BuyingRate { get; set; }
        public double SellingRate { get; set; }
        public long Unit { get; set; }
    }
}