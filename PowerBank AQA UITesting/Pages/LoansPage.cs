using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "LoansPage", Url = "account/loans")]
    public class LoansPage : Page
    {
        [Element(Name = "Лого кредита", Locator = "//*[@src = '/cc300ebd3de83d06bad0.png']")]
        Element logoLoanProduct;

        [Element(Name = "Подробнее кредит классический", Locator = "//span[text()='Классический']/../div[2]/button[text()='Подробнее']")]
        Button moreButtonClassicLoan;

        [Element(Name = "Подробнее кредит срочный", Locator = "//span[text()='Срочный']/../div[2]/button[text()='Подробнее']")]
        Button moreButtonUrgentLoan;

        [Element(Name = "Подробнее кредит авто", Locator = "//span[text()='Покупка автомобиля']/../div[2]/button[text()='Подробнее']")]
        Button moreButtonAutoLoan;
    }
}
