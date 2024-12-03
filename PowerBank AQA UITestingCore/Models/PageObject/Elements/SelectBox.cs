using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public class SelectBox : DefaultClick
    {
        public SelectBox(string name, string locator, string boxXpath, string childrenXpath, bool optional = false) 
            : base(name, locator, optional)
        {
        }
    }
}
