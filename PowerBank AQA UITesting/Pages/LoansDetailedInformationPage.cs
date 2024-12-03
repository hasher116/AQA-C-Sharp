using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "LoansPageDetailedInformation", Url = "account/loans/details/")]
    public class LoansDetailedInformationPage : Page
    {
        [Element(Name = "Условие кредитного договора Классический", Locator = "//a[contains(text(),'Условие кредитного договора \"Классический\"')]")]
        Button linkConditionsOfLoanFamily;

        [Element(Name = "Условие кредитного договора Срочный", Locator = "//a[contains(text(),'Условие кредитного договора \"Срочный\"')]")]
        Button linkConditionsOfLoanQuick;

        [Element(Name = "Условие кредитного договора Авто", Locator = "//a[contains(text(),'Условие кредитного договора \"Авто\"')]")]
        Button linkConditionsOfLoanClassic;

        [Element(Name = "Название кредита", Locator = "//span[@class='MuiTypography-root MuiTypography-subtitle8 css-mjbrcq']")]
        Element depositNameLabel;

        [Element(Name = "Ставка годовых значение", Locator = "//p[contains(text(),'Ставка годовых')]/following-sibling::*")]
        Element annualRateLabel;

        [Element(Name = "Минимальная сумма значение", Locator = "//p[contains(text(),'Минимальная сумма')]/following-sibling::*")]
        Element minSumLabel;

        [Element(Name = "Максимальная сумма значение", Locator = "//p[contains(text(),'Максимальная сумма')]/following-sibling::*")]
        Element maxSumLabel;

        [Element(Name = "Минимальный срок значение", Locator = "//p[contains(text(),'Минимальный срок')]/following-sibling::*")]
        Element minDurationLabel;

        [Element(Name = "Максимальный срок значение", Locator = "//p[contains(text(),'Максимальный срок')]/following-sibling::*")]
        Element maxDurationLabel;

        [Element(Name = "Досрочное погашение значение", Locator = "//p[contains(text(),'Досрочное погашение')]/following-sibling::*")]
        Element earlyRepainmentLabel;

        [Element(Name = "Поручители значение", Locator = "//p[contains(text(),'Поручители')]/following-sibling::*")]
        Element guarantorsLabel;

        [Element(Name = "Документы значение", Locator = "//p[contains(text(),'Документы')]/following-sibling::*")]
        Element documentsLabel;

        [Element(Name = "Кнопка назад", Locator = "//*[contains(text(),'Назад')]")]
        Button back;
    }
}
