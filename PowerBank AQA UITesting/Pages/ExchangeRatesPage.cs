using Microsoft.VisualBasic;
using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "ExchangeRatesPage", Url = "currency")]
    public class ExchangeRatesPage : Page
    {
        [Element(Name = "CardProducts", Locator = "//h3[contains(@class, 'MuiTypography-root') and text()='Курсы валют']")]
        Element labelExchangeRates;

        // USD
        [Element(Name = "USD", Locator = "//div[@data-testid='USD']")]
        Element dataUSD;

        [Element(Name = "Код USD", Locator = "//div[@data-testid='USD']/div[2]/span[text()='USD']")]
        Element codeUSD;

        [Element(Name = "Покупка USD", Locator = "//div[@data-testid='USD'][2]")]
        Element buyingUSD;

        [Element(Name = "Продажа USD", Locator = "//div[@data-testid='USD'][3]")]
        Element sellingUSD;

        // EUR
        [Element(Name = "EUR", Locator = "//div[@data-testid='EUR']")]
        Element dataEUR;

        [Element(Name = "Код EUR", Locator = "//div[@data-testid='EUR']/div[2]/span[text()='EUR']")]
        Element codeEUR;

        [Element(Name = "Покупка EUR", Locator = "//div[@data-testid='EUR'][2]")]
        Element buyingEUR;

        [Element(Name = "Продажа EUR", Locator = "//div[@data-testid='EUR'][3]")]
        Element sellingEUR;

        // CNY
        [Element(Name = "CNY", Locator = "//div[@data-testid='CNY']")]
        Element dataCNY;

        [Element(Name = "Код CNY", Locator = "//div[@data-testid='CNY']/div[2]/span[text()='CNY']")]
        Element codeCNY;

        [Element(Name = "Покупка CNY", Locator = "//div[@data-testid='CNY'][2]")]
        Element buyingCNY;

        [Element(Name = "Продажа CNY", Locator = "//div[@data-testid='CNY'][3]")]
        Element sellingCNY;

        // CAD
        [Element(Name = "CAD", Locator = "//div[@data-testid='CAD']")]
        Element dataCAD;

        [Element(Name = "Код CAD", Locator = "//div[@data-testid='CAD']/div[2]/span[text()='CAD']")]
        Element codeCAD;

        [Element(Name = "Покупка CAD", Locator = "//div[@data-testid='CAD'][2]")]
        Element buyingCAD;

        [Element(Name = "Продажа CAD", Locator = "//div[@data-testid='CAD'][3]")]
        Element sellingCAD;

        // GBP
        [Element(Name = "GBP", Locator = "//div[@data-testid='GBP']")]
        Element dataGBP;

        [Element(Name = "Код GBP", Locator = "//div[@data-testid='GBP']/div[2]/span[text()='GBP']")]
        Element codeGBP;

        [Element(Name = "Покупка GBP", Locator = "//div[@data-testid='GBP'][2]")]
        Element buyingGBP;

        [Element(Name = "Продажа GBP", Locator = "//div[@data-testid='GBP'][3]")]
        Element sellingGBP;

        // JPY
        [Element(Name = "JPY", Locator = "//div[@data-testid='JPY']")]
        Element dataJPY;

        [Element(Name = "Код JPY", Locator = "//div[@data-testid='JPY']/div[2]/span[text()='JPY']")]
        Element codeJPY;

        [Element(Name = "Покупка JPY", Locator = "//div[@data-testid='JPY'][2]")]
        Element buyingJPY;

        [Element(Name = "Продажа JPY", Locator = "//div[@data-testid='JPY'][3]")]
        Element sellingJPY;
    }
}