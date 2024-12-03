using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_SpecFlow.Pages.Forms
{
    [Page(PageName = "LeftMenu", Url = "account")]
    public class LeftMenu : Page
    {
        [Element(Name = "Главная", Locator = "//div[@class='MuiBox-root css-vcfpbk']/a[contains(@href,'account/main'")]
        Button main;

        [Element(Name = "Карты", Locator = "//a[contains(@class, 'MuiBox-root') and contains(@href, 'account/cards')]")]
        Button cards; 

        [Element(Name = "Депозиты", Locator = "//div[@class='MuiBox-root css-vcfpbk']/a[contains(@href,'/account/deposits')]")]
        Button deposits;

        [Element(Name = "Кредиты", Locator = "//div[@class='MuiBox-root css-vcfpbk']/a[contains(@href,'/account/loans')]")]
        Button loans;

        [Element(Name = "Страхование", Locator = "//div[@class='MuiBox-root css-vcfpbk']/a[contains(@href,'/account/insurance')]")]
        Button insurance;

        [Element(Name = "Инвестиции", Locator = "//div[@class='MuiBox-root css-vcfpbk']/a[contains(@href,'/account/investments')]")]
        Button investments;

        [Element(Name = "Акции", Locator = "//div[@class='MuiBox-root css-vcfpbk']/a[contains(@href,'/account/deals')]")]
        Button deals;

        [Element(Name = "Выход", Locator = "//p[contains(@class, css-qkqikj)] [contains(text(), 'Выход')]")]
        Button exit;
    }
}
