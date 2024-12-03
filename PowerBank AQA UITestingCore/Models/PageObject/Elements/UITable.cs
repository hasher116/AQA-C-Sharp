using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public class UITable : Element
    {
        public UITable(string name, string locator, string headerByXpath, string headerCellByXpath, 
            string itemByXpath, string itemCellByXpath, bool optional = false) 
            : base(name, locator, optional)
        {
        }
    }
}
