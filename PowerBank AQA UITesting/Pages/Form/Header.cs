using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages.Form
{
    [Page(PageName = "Header")]
    public class Header : Page
    {
        [Element(Name = "Главная страница", Locator = "//a[contains(@class, 'MuiTypography-root') and @href='/account']")]
        Button buttonMainPage;

        [Element(Name = "Банкоматы и отделения", Locator = "//a[contains(@class, 'MuiTypography-root') and @href='/departments']")]
        Button buttonDepartment;

        [Element(Name = "Курсы валют", Locator = "//a[contains(@class, 'MuiTypography-root') and @href='/currency']")]
        Button buttonExchangeRates;

        [Element(Name = "Контакты", Locator = "//a[contains(@class, 'MuiTypography-root') and @href='/contacts']")]
        Button buttonContacts;

        [Element(Name = "Настройки пользователя", Locator = "//a[contains(@class, 'MuiTypography-root') and @href='/settings']")]
        Button buttonUserSettings;
    }
}
