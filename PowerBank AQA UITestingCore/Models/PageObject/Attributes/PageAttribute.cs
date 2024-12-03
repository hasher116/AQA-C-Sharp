using PowerBank_AQA_TestingCore.Configuration;
using System;
using System.Globalization;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageAttribute : Attribute
    {
        private string url;
        public string PageName { get; set; }
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = Configuration.GetConfiguration()["baseUrl"].ToString() + value;
            }
        }
    }
}
