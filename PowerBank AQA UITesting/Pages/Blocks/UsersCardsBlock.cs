using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITesting.Pages.Blocks
{
    public class UsersCardsBlock : Block
    {
        public UsersCardsBlock(string name, string locator, bool optional = false) : base(name, locator, optional)
        {
        }

        [Element(Name = "Карточка SummerOffer", Locator = "//*[@class = 'MuiBox-root css-19h296d']")]
        Button cardSummerOffer;
    }
}
