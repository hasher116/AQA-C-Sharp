using Microsoft.VisualBasic;
using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_SpecFlow.Pages
{
    [Page(PageName = "CardPage", Url = "account/cards")]
    public class CardPage : Page
    {
        // TypeCardButtons
        [Element(Name = "Дебетовая карта", Locator = "//a[@id='simple-tab-0']")]
        Button buttonCardDebitProducts;

        [Element(Name = "Кредитная карта", Locator = "//a[@id='simple-tab-1']")]
        Button buttonCardCreditProducts;

        [Element(Name = "Виртуальная карта", Locator = "//a[@id='simple-tab-2']")]
        Button buttonCardVirtualProducts;

        //DebitCards
        [Element(Name = "Solid Card", Locator = "//p[text()='Solid Card']/../preceding-sibling::div")]
        Button buttonCardSolid;

        [Element(Name = "Power Drive", Locator = "//p[text()='Power Drive']/../preceding-sibling::div")]
        Button buttonPowerDrive;

        [Element(Name = "Airlines", Locator = "//p[text()='Airlines']/../preceding-sibling::div")]
        Button buttonAirlines;

        //CreditCards
        [Element(Name = "Shopping Card", Locator = "//p[text()='Shopping Card']/../preceding-sibling::div")]
        Button buttonCardShopping;

        [Element(Name = "Cache Card", Locator = "//p[text()='Cache Card']/../preceding-sibling::div")]
        Button buttonCardCache;

        //VirtualCards
        [Element(Name = "Neon Card", Locator = "//p[text()='Neon Card']/../preceding-sibling::div")]
        Button buttonCardNeon;

        [Element(Name = "Lady Card", Locator = "//p[text()='Lady Card']/../preceding-sibling::div")]
        Button buttonCardLady;

        [Element(Name = "Super Card", Locator = "//p[text()='Super Card']/../preceding-sibling::div")]
        Button buttonCardSuper;

        //CommonInfo
        //GameCard
        [Element(Name = "Game Card", Locator = "//p[text()='Game Card']")]
        Element LabelCardGame;

        [Element(Name = "Тип Game Card", Locator = "//p[text()='Game Card']/preceding-sibling::p")]
        Element LabelTypeCardGame;

        [Element(Name = "Описание Game Card", Locator = "//p[contains(text(), 'Безопасная и простая')]")]
        Element LabelDescriptionCardGame;

        [Element(Name = "Кешбэк Game Card", Locator = "//p[contains(text(), 'Безопасная и простая')]/../div/div/p[text()='кешбэк']/preceding-sibling::p")]
        Element LabelCashBackMaxCardGame;

        [Element(Name = "Обслуживание Game Card", Locator = "//p[contains(text(), 'Безопасная и простая')]/../div/div/p[contains(text(), 'обслуживание')]/preceding-sibling::p")]
        Element LabelPriceServiceCardGame;

        //CacheCard
        [Element(Name = "Лэйбл Cache Card", Locator = "//p[text()='Cache Card']")]
        Element cardCache;

        [Element(Name = "Тип Cache Card", Locator = "//p[text()='Cache Card']/preceding-sibling::p")]
        Element LabelTypeCardCache;

        [Element(Name = "Описание Cache Card", Locator = "//p[contains(text(), 'Не время')]")]
        Element LabelDescriptionCacheGame;

        [Element(Name = "Процент годовых Cache Card", Locator = "//p[contains(text(), 'Не время')]/../div/div/p[text()='годовых']/preceding-sibling::p")]
        Element LabelInterestRateCacheGame;

        [Element(Name = "Беспроцентный период Cache Card", Locator = "//p[contains(text(), 'Не время')]/../div/div/p[text()='беспроцентный период']/preceding-sibling::p")]
        Element LabelInterestFreeDaysCacheGame;

        [Element(Name = "Стоимость обслуживания Cache Card", Locator = "//p[contains(text(), 'Не время')]/../div/div/p[text()='обслуживание в месяц']/preceding-sibling::p")]
        Element LabelServicePriceCacheGame;

        // PowerCard
        [Element(Name = "Power Card", Locator = "//p[text()='Power Card']")]
        Element cardPower;

        [Element(Name = "Тип Power Card", Locator = "//p[text()='Power Card']/preceding-sibling::p")]
        Element LabelTypeCardPower;

        [Element(Name = "Описание Power Card", Locator = "//p[contains(text(), 'Для тех')]")]
        Element LabelDescriptionCardPower;

        [Element(Name = "Кешбэк Power Card", Locator = "//p[contains(text(), 'Для тех')]/../div/div/p[text()='кешбэк']/preceding-sibling::p")]
        Element LabelCashBackMaxCardPower;

        [Element(Name = "Обслуживание Power Card", Locator = "//p[contains(text(), 'Для тех')]/../div/div/p[contains(text(), 'обслуживание')]/preceding-sibling::p")]
        Element LabelPriceServiceCardPower;
    }
}
