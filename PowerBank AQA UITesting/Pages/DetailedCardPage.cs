using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;
using System.Reflection.Emit;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "DetailedCardPage")]
    public class DetailedCardPage : Page
    {
        [Element(Name = "CardLabel", Locator = "//div[@data-testid = 'backLinkContainer']/span")]
        Element cardLabel;

        [Element(Name = "CardType", Locator = "//div[@data-testid='backLinkContainer']/..//*[local-name()='svg']/../p")]
        Element cardType;

        [Element(Name = "CardCashBack", Locator = "//p[contains(@class, MuiTypography-root) and text()='Кешбэк в месяц']/../p[2]")]
        Element cardCashBack;

        [Element(Name = "CardServicePrice", Locator = "//p[contains(@class, MuiTypography-root) and text()='Стоимость обслуживания в месяц']/../p[2]")]
        Element cardServicePrice;

        [Element(Name = "CardCashMaxDay", Locator = "//p[contains(@class, MuiTypography-root) and text()='Лимит на выдачу наличных в день']/../p[2]")]
        Element cardCashMaxDay;

        [Element(Name = "CardCashMaxMonth", Locator = "//p[contains(@class, MuiTypography-root) and text()='Лимит на выдачу наличных в месяц']/../p[2]")]
        Element cardCashMaxMonth;

        [Element(Name = "CardNotificationPrice", Locator = "//p[contains(@class, MuiTypography-root) and text()='Стоимость СМС-уведомлений в месяц']/../p[2]")]
        Element cardNotificationPrice;

        [Element(Name = "CardPaymentSystem", Locator = "//p[contains(@class, MuiTypography-root) and text()='Платежная система']/../p[2]")]
        Element cardPaymentSystem;

        [Element(Name = "CardInterestRate", Locator = "//p[contains(@class, MuiTypography-root) and text()='Процентная ставка']/../p[2]")]
        Element cardInterestRate;

        [Element(Name = "CardInterestFreeDays", Locator = "//p[contains(@class, MuiTypography-root) and text()='Беспроцентный период']/../p[2]")]
        Element cardInterestFreeDays;

        [Element(Name = "CardAmountCreditMax", Locator = "//p[contains(@class, MuiTypography-root) and text()='Кредитный лимит']/../p[2]")]
        Element cardAmountCreditMax;

        [Element(Name = "CardCashWithdrawalFee", Locator = "//p[contains(@class, MuiTypography-root) and text()='Комиссия за выдачу наличных']/../p[2]")]
        Element cardCashWithdrawalFee;

        [Element(Name = "CardTariff", Locator = "//a[text()='Тарифы по карте']")]
        Button cardTariff;

        [Element(Name = "CardService", Locator = "//a[text()='Условия выпуска и обслуживания']")]
        Button cardService;
    }
}
