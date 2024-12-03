namespace PowerBank_AQA_UITestingCore.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SelectBoxAttribute : ElementAttribute
    {
        public string ChildrenXpath { get; set; }

        public string BoxXpath { get; set; }
    }
}
