using Microsoft.VisualBasic;
using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "CardsPage", Url = "account/cards")]
    public class CardsPage : Page
    {
        [Element(Name = "CardProducts", Locator = "//h3[contains(@class, 'MuiTypography-root') and contains(text(), 'Карточные продукты')]")]
        Element cardProducts;

        // Cards

        [Element(Name = "CardUser", Locator = "//span[text()='Мои карты']/../../../div[2]/div[1]")]
        Button buttonCardUser;

        // Order Statement
        
        [Element(Name = "AccountStatement", Locator = "//button[text()='Выписка по счёту']")]
        Button buttonAccountStatement;

        [Element(Name = "OrderAccountStatement", Locator = "//span[text()='Заказ выписки со счета']")]
        Element labelOrderAccountStatement;

        [Element(Name = "Back", Locator = "//a[@data-testid='backLink']")]
        Button buttonBack;

        [Element(Name = "Enter period", Locator = "//input[@type='text']")]
        Input inputEnterPeriod;

        [Element(Name = "1 Month", Locator = "//div[@role='radiogroup']/label[1]")]
        RadioButton radioButtonOneMonth;

        [Element(Name = "2 Month", Locator = "//div[@role='radiogroup']/label[2]")]
        RadioButton radioButtonTwoMonth;

        [Element(Name = "6 Month", Locator = "//div[@role='radiogroup']/label[3]")]
        RadioButton radioButtonSixMonth;

        [Element(Name = "Calendar", Locator = "//input[@type='text']/../div/button[@tabindex='0']")]
        Button buttonCalendar;

        [Element(Name = "Submit", Locator = "//button[@type='submit']")]
        Button buttonSubmit;

        [Element(Name = "Period", Locator = "//span[text()='Период:']")]
        Element textPeriod;

        [Element(Name = "Format PDF", Locator = "//p[text()='Сохранить в PDF']")]
        Element textPDF;

        [Element(Name = "Format XLSX", Locator = "//p[text()='Сохранить в XLSX']")]
        Element textXLSX;

        [Element(Name = "Format TXT", Locator = "//p[text()='Сохранить в TXT']")]
        Element textTXT;

        [Element(Name = "Open and look", Locator = "//p[text()='Открыть и посмотреть']/..")]
        Element textOpenAndLook;

        [Element(Name = "Start date", Locator = "//div[@aria-rowindex='2']/div[1]/button")]
        Button buttonStartDate;

        [Element(Name = "End date", Locator = "//div[@aria-rowindex='2']/div[5]/button")]
        Button buttonEndDate;

        [Element(Name = "Previous month", Locator = "//button[@title='Previous month']")]
        Button buttonPreviousMonth;

        [Element(Name = "Next month", Locator = "//button[@title='Next month']")]
        Button buttonNextMonth;

        [Element(Name = "Accept calendar date", Locator = "//button[text()='Применить']")]
        Button buttonAcceptCalendarDate;

        [Element(Name = "Account number on page", Locator = "//p[text()='Номер счета']/../p[2]/div")]
        Element textAccountNumberOnPage;

        [Element(Name = "Account number in statement", Locator = "//span[contains(text(), 'Выписка по номеру')]")]
        Element textAccountNumberInStatement;

        // Common
        [Element(Name = "CardLabel", Locator = "//button[contains(text(),'Подробнее')]/../p[2]")]
        Element cardLabel;

        [Element(Name = "CardType", Locator = "//button[contains(text(),'Подробнее')]/../p[1]")]
        Element cardType;

        [Element(Name = "CardShortDescription", Locator = "//button[contains(text(),'Подробнее')]/../../..//p[contains(@class,'MuiTypography-root MuiTypography-body2')]")]
        Element cardShortDescription;

        [Element(Name = "CardCashback", Locator = "//p[contains(text(),'кешбэк')]/../p[1]")]
        Element cardCashback;

        [Element(Name = "CardServicePrice", Locator = "//p[contains(text(),'обслуживание в месяц')]/../p[1]")]
        Element cardServicePrice;

        [Element(Name = "CardPaymentSystem", Locator = "//p[text()='Solid Card']/../..//*[local-name()='svg']/*[local-name()='path' and @fill='url(#paint0_linear_9908_59106)']")]
        Element cardPaymentSystem;

        [Element(Name = "CardInterestRate", Locator = "//p[contains(@class, 'MuiTypography') and text()='годовых']/../p[1]")]
        Element cardInterestRate;

        [Element(Name = "CardInterestFreeDay", Locator = "//p[contains(@class, 'MuiTypography') and text()='беспроцентный период']/../p[1]")]
        Element cardInterestFreeDay;


        // TypeCardButtons
        [Element(Name = "ButtonDebit", Locator = "//a[@id='simple-tab-0']")]
        Button buttonCardDebitProducts;

        [Element(Name = "ButtonCredit", Locator = "//a[@id='simple-tab-1']")]
        Button buttonCardCreditProducts;

        [Element(Name = "ButtonVirtual", Locator = "//a[@id='simple-tab-2']")]
        Button buttonCardVirtualProducts;

        // DebitCards
        [Element(Name = "LabelCardSolid", Locator = "//p[text()='Solid Card']")]
        Element labelCardSolid;

        [Element(Name = "ButtonCardSolid", Locator = "//p[text()='Solid Card']/../../div")]
        Button buttonCardSolid;

        [Element(Name = "LabelCardGame", Locator = "//p[text()='Game Card']")]
        Element labelCardGame;

        [Element(Name = "ButtonCardGame", Locator = "//p[text()='Game Card']/../../div")]
        Button buttonCardGame;

        [Element(Name = "LabelCardPowerDrive", Locator = "//p[text()='Power Drive']")]
        Element labelCardPowerDrive;

        [Element(Name = "ButtonCardPowerDrive", Locator = "//p[text()='Power Drive']/../../div")]
        Button buttonCardPowerDrive;

        [Element(Name = "LabelCardAirlines", Locator = "//p[text()='Airlines']")]
        Element labelCardAirlines;

        [Element(Name = "ButtonCardAirlines", Locator = "//p[text()='Airlines']/../../div")]
        Button buttonCardAirlines;

        // CreditCards
        [Element(Name = "LabelCardShopping", Locator = "//p[text()='Shopping Card']")]
        Element labelCardShopping;

        [Element(Name = "ButtonCardShopping", Locator = "//p[text()='Shopping Card']/../../div")]
        Button buttonCardShopping;

        [Element(Name = "LabelCardCache", Locator = "//p[text()='Cache Card']")]
        Element labelCardCache;

        [Element(Name = "ButtonCardCache", Locator = "//p[text()='Cache Card']/../../div")]
        Button buttonCardCache;

        // VirtualCards
        [Element(Name = "LabelCardSuper", Locator = "//p[text()='Super Card']")]
        Element labelCardSuper;

        [Element(Name = "ButtonCardSuper", Locator = "//p[text()='Super Card']/../../div")]
        Button buttonCardSuper;

        [Element(Name = "LabelCardPower", Locator = "//p[text()='Power Card']")]
        Element labelCardPower;

        [Element(Name = "ButtonCardPower", Locator = "//p[text()='Power Card']/../../div")]
        Button buttonCardPower;

        [Element(Name = "LabelCardLady", Locator = "//p[text()='Lady Card']")]
        Element labelCardLady;

        [Element(Name = "ButtonCardLady", Locator = "//p[text()='Lady Card']/../../div")]
        Button buttonCardLady;

        [Element(Name = "LabelCardNeon", Locator = "//p[text()='Neon Card']")]
        Element labelCardNeon;

        [Element(Name = "ButtonCardNeon", Locator = "//p[text()='Neon Card']/../../div")]
        Button buttonCardNeon;

        // Переводы
        [Element(Name = "Переход на карту 3485", Locator = "//p[contains(text(), '3485')]/../..")] 
        Button buttonCardIHaveSixCards;

        [Element(Name = "Дебетовая карта 3485", Locator = "//p[contains(text(), '3485')]")]
        Button labelCardIHaveSixCards;

        [Element(Name = "Оплатить и перевести", Locator = "//button[text()='Оплатить и перевести']")]
        Button buttonPayAndTransfer;

        [Element(Name = "Перевод на карту", Locator = "//a[contains(@href, 'other-card')]/button")]
        Button buttonTransferToCard;

        [Element(Name = "Введите номер карты", Locator = "//input[@name='recipientCardNumber']")]
        Input inputEnterNumberCard;

        [Element(Name = "Продолжить", Locator = "//button[@data-testid='confirmButton']")]
        Button buttonConfirmButton;

        [Element(Name = "Введите сумму", Locator = "//input[@name='amountTransfer']")]
        Input inputEnterAmount;

        [Element(Name = "Введите код", Locator = "//input[@name='cvvCode']")]
        Input inputEnterCVV;

        [Element(Name = "Введите комментарий", Locator = "//input[@name='transferComment']")]
        Input inputEnterComment;

        [Element(Name = "Перевести", Locator = "//button[@type='submit']")]
        Button buttonSubmitTransfer;

        [Element(Name = "Актуальная карта отправителя", Locator = "//p[text()='С карты']/following-sibling::p")]
        Element labelActualCardSender;

        [Element(Name = "Ожидаемая карта отправителя", Locator = "//p[text()='Номер карты']/following-sibling::p/div")]
        Element labelExpectedCardSender;

        [Element(Name = "Актуальная карта получателя", Locator = "//p[text()='На карту']/following-sibling::p")]
        Element labelActualCardRecipient;

        [Element(Name = "Сумма перевода", Locator = "//p[text()='Сумма']/following-sibling::p")]
        Element labelAmountTransfer;

        [Element(Name = "Полная сумма перевода", Locator = "//p[text()='Итого']/following-sibling::p")]
        Element labelFullAmountTransfer;

        [Element(Name = "Статус перевода", Locator = "//p[text()='Статус']/following-sibling::p")]
        Element labelStatusTransfer;

        [Element(Name = "Завершение перевода", Locator = "//button[@type='submit' and text()='Продолжить']")]
        Button buttonEndTransfer;

        [Element(Name = "Ошибка ввода карты", Locator = "//div[@data-test-id='recipientCardNumberInput']/following-sibling::p")]
        Element labelErrorCardNumberInput;

        [Element(Name = "Чекбокс отправка на почту", Locator = "//p[text()='Отчет о переводе']/following-sibling::label")]
        CheckBox checkBoxSendEmail;

        [Element(Name = "Поле ввода email", Locator = "//input[@name='email']")]
        Input inputEmail;

        [Element(Name = "Результат отправки отчёта", Locator = "//p[text()='E-mail']/following-sibling::p")]
        Element labelResultSendReport;

        [Element(Name = "Недостаточно средств", Locator = "//p[contains(text(), 'Недостаточно средств')]")]
        Element labelErrorNotEnough;
    }
}
