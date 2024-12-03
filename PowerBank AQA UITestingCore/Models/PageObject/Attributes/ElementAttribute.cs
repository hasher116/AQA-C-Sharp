using PowerBank_AQA_UITestingCore.Infrastructures;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ElementAttribute : Attribute
    {
        public string Name { get; set; }

        public How How { get; set; } = How.XPath;

        public string Locator { get; set; }

        public bool Optional { get; set; }

        public bool Global { get; set; }
    }
}
