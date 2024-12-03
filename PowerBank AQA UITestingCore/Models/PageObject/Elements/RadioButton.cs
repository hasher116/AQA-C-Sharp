using System;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public class RadioButton : DefaultClick
    {
        public RadioButton(string name, string locator, bool optional = false) : base(name, locator, optional)
        {
        }
    }
}
