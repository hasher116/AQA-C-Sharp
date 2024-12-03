using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "DetailedDepositInfomation", Url = "account/deposits/order/")]
    public class DetailedDepositInformation : Page
    {
        [Element(Name = "Условие кредитного договора Семейный", Locator = "//a[contains(text(),'Условие кредитного договора \"Семейный\"')]")]
        Button linkConditionsOfLoanFamily;
    }
}
