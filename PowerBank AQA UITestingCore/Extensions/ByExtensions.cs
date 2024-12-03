using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Infrastructures;

namespace PowerBank_AQA_UITestingCore.Extensions
{
    public static class ByExtensions
    {
        public static By GetBy(this How how, string @using) =>
            how switch
            { 
                How.Id => By.Id(@using),
                How.Name => By.Name(@using),
                How.TagName => By.TagName(@using),
                How.ClassName => By.ClassName(@using),
                How.PartialLinkText => By.PartialLinkText(@using),
                How.CssSelector => By.CssSelector(@using),
                How.LinkText => By.LinkText(@using),
                How.XPath => By.XPath(@using),
                _ => throw new ArgumentOutOfRangeException(nameof(how), how, null)
            };
    }
}
