using System;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public class CheckBox : DefaultClick
    {
        public CheckBox(string name, string locator, bool optional = false) : base(name, locator, optional)
        {
        }
    }
}
