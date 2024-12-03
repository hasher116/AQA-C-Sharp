namespace PowerBank_AQA_UITestingCore.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TableAttribute : ElementAttribute
    {
        public string HeaderXPath { get; set; }

        public string BodyXPath { get; set; }

        public string ItemHeaderByXPath { get; set; }

        public string ItemBodyXPath { get; set; }
    }
}
