namespace PowerBank_AQA_UITestingCore.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FrameAttribute : ElementAttribute
    {
        public int? Number { get; set; } = default;

        public string FrameName { get; set; }
    }
}
