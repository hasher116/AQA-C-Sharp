using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;
using System.Xml.XPath;

namespace PowerBank_AQA_UITesting.Pages.Form
{
    [Page(PageName = "LeftMenu", Url = "account")]
    public class LeftMenu : Page
    {
        [Element(Name = "Главная", Locator = "//a[contains(@href, 'account/main')]")]
        Button main;

        [Element(Name = "Карты", Locator = "//a[contains(@href, 'account/cards')]")]
        Button cards; 

        [Element(Name = "Депозиты", Locator = "//a[contains(@href, 'account/deposits')]")]
        Button deposits;

        [Element(Name = "Кредиты", Locator = "//a[contains(@href, 'account/loans')]")]
        Button loans;

        [Element(Name = "Страхование", Locator = "//a[contains(@href, 'account/insurance')]")]
        Button insurance;

        [Element(Name = "Инвестиции", Locator = "//a[contains(@href, 'account/investments')]")]
        Button investments;

        [Element(Name = "Акции", Locator = "//a[contains(@href, 'account/deals')]")]
        Button deals;

        [Element(Name = "Выход", Locator = "//div[@tabindex='0']")]
        Button exit;
    }
}
