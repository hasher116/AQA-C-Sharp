using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "OpenedDepositPage", Url = "account/deposits/")]
    public class OpenedDepositPage : Page
    {
        [Element(Name = "Кнопка назад", Locator = "//*[contains(text(),'Назад')]")]
        Button back;

        [Element(Name = "Свитчер Автопродление", Locator = "//input[@type='checkbox']")]
        Button switcherProlongation;

        [Element(Name = "Свитчер Автопродление неактивный", Locator = "//span[contains(@class, 'MuiSwitch-switchBase')]")]
        Button switcherOff;

        [Element(Name = "Свитчер Автопродление активный", Locator = "//span[contains(@class, 'Mui-checked')]")]
        Button switcherOn;

        [Element(Name = "Свитчер Автопродление включить", Locator = "//span[contains(@class, 'MuiSwitch-root MuiSwitch-sizeMedium')]")]
        Button switcherPush;

        [Element(Name = "Форма договора", Locator = "//a[contains(text(),'Форма договора')]")]
        Button contractForm;

        [Element(Name = "Условия депозитного продукта", Locator = "//a[contains(text(),'Условия депозитного продукта')]")]
        Button depositConditions;

        [Element(Name = "Отозвать депозит", Locator = "//button[text()='Отозвать депозит']")]
        Button withdrawDeposit;

        [Element(Name = "Список выбора карт после 'Отозвать депозит'", Locator = "//div[@tabIndex='0' and @role='button']")]
        Button withdrawDepositCardsToReplenishment;

        [Element(Name = "Поле 'Юридический адрес и Контакты'", Locator = "//p[text()='Юридический адрес:']")]
        Element adressContactsLabel;

        [Element(Name = "'Юридический адрес и Контакты' - значения", Locator = "//p[text()='Юридический адрес:']/../following-sibling::div")]
        Element adressContactsValueLabel;

        [Element(Name = "Валюта депозита", Locator = "//*[contains(text(),'Валюта депозита')]/following-sibling::*")]
        Element depositCurrency;

        [Element(Name = "Дата открытия", Locator = "//*[contains(text(),'Дата открытия')]/following-sibling::*")]
        Element openDate;

        [Element(Name = "Дата закрытия", Locator = "//*[contains(text(),'Дата закрытия')]/following-sibling::*")]
        Element closeDate;

        [Element(Name = "Срок действия", Locator = "//*[contains(text(),'Срок действия')]/following-sibling::*")]
        Element validity;

        [Element(Name = "Сумма вклада", Locator = "//*[contains(text(),'Сумма вклада')]/following-sibling::*")]
        Element depositAmount;

        [Element(Name = "Минимальная сумма вклада", Locator = "//*[contains(text(),'Минимальная сумма вклада')]/following-sibling::*")]
        Element minDepositAmount;

        [Element(Name = "Процентная ставка", Locator = "//*[contains(text(),'Процентная ставка')]/following-sibling::*")]
        Element interestRate;

        [Element(Name = "Текущее состояние", Locator = "//*[contains(text(),'Текущее состояние')]/following-sibling::*")]
        Element currentState;

        [Element(Name = "Автопродление", Locator = "//*[contains(text(),'Автопродление')]/following-sibling::*")]
        Element autoRenewal;

        [Element(Name = "Капитализация", Locator = "//*[contains(text(),'Капитализация')]/following-sibling::*")]
        Element capitalization;

        [Element(Name = "Досрочное закрытие", Locator = "//*[contains(text(),'Досрочное закрытие')]/following-sibling::*")]
        Element earlyClosure;
    }
}
