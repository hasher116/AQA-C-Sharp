using FluentAssertions;
using NUnit.Allure.Attributes;
using PowerBank_AQA_UITesting.Hooks;
using PowerBank_AQA_UITestingCore.Models.Factory.Browser;
using PowerBank_AQA_UITestingCore.Models.Settings;
using System.Data;
using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_UITesting.Dto;
using Bogus;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_DbTestingCore.DbClient;
using DbQueries = PowerBank_AQA_UITesting.Hooks.DbQueries;
using System.Globalization;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using MongoDB.Bson;

namespace PowerBank_AQA_UITesting.Steps
{
    public class CommonSteps
    {
        private readonly IBrowser browser;
        private readonly ElementSteps elementSteps;
        private readonly Settings settings;

        public CommonSteps(IBrowser browser, ElementSteps elementSteps, Settings settings)
        {
            this.browser = browser;
            this.settings = settings;
            this.elementSteps = elementSteps;
        }

        [AllureStep("Авторизация")]
        public void Authorization(string loginPhone, string password)
        {
            elementSteps.SendKeys("Поле ввода для телефона", loginPhone);
            elementSteps.SendKeys("Поле ввода для пароля", password);
            elementSteps.ClickToWebElement("Войти");
        }

        [AllureStep("Проверка депозита; карточка: {1} название депозита: {2}")]
        public void CheckDepositExistsOnPage(DataTable dataTable, string cardElement, string depositNameOnPage,
            string columnName)
        {
            DbHelper dbHelper = new DbHelper();
            elementSteps.IsWebElementDisplayed(cardElement).Should().BeTrue();
            var depositName = elementSteps.GetText(depositNameOnPage);
            Log.Logger().LogInformation($"{depositName} +  fromPage");
            dbHelper.IsDataNameExistsInColumn(dataTable, columnName, depositName).Should().BeTrue();
        }

        [AllureStep("Проверка наличия карт на странице")]
        public void CheckCardExistsOnPage(DataTable dataTable, string cardElement, string cardNameOnPage,
            string columnName)
        {
            DbHelper dbHelper = new DbHelper();
            elementSteps.IsWebElementDisplayed(cardElement).Should().BeTrue(cardElement);
            var cardName = elementSteps.GetText(cardNameOnPage);
            Log.Logger().LogInformation($"{cardName} +  fromPage");
            dbHelper.IsDataNameExistsInColumn(dataTable, columnName, cardName).Should().BeTrue();
        }

        [AllureStep("Проверка краткой информации о дебитовой и виртуальной карте")]
        public void CheckShortInfoAboutCardDebitAndVirtual(DataTable dataTable, CardShortInformation actualCard)
        {
            DbHelper dbHelper = new DbHelper();
            CardShortInformation expectedCard = dbHelper.DataTableToCardDebitAndVirtualShortInformation(dataTable);

            expectedCard.Equals(actualCard).Should().BeTrue();
        }

        [AllureStep("Проверка краткой информации о кредитной карте")]
        public void CheckShortInfoAboutCardCredit(DataTable dataTable, CardShortInformation actualCard)
        {
            DbHelper dbHelper = new DbHelper();
            CardShortInformation expectedCard = dbHelper.DataTableToCardCreditShortInformation(dataTable);

            expectedCard.Equals(actualCard).Should().BeTrue();
        }

        [AllureStep("Проверка подробной информации о дебетовой карте")]
        public void CheckDetailedInfoAboutCardDebit(DataTable dataTable, CardDetailedInformation actualCard)
        {
            DbHelper dbHelper = new DbHelper();
            CardDetailedInformation expectedCard = dbHelper.DataTableToCardDebitDetailedInformation(dataTable);
            expectedCard.CardTariff = elementSteps.IsExistsLink("CardTariff");
            expectedCard.CardService = elementSteps.IsExistsLink("CardService");

            expectedCard.Equals(actualCard).Should().BeTrue();
        }

        [AllureStep("Проверка подробной информации о кредитной карте")]
        public void CheckDetailedInfoAboutCardCredit(DataTable dataTable, CardDetailedInformation actualCard)
        {
            DbHelper dbHelper = new DbHelper();
            CardDetailedInformation expectedCard = dbHelper.DataTableToCardCreditDetailedInformation(dataTable);
            expectedCard.CardTariff = elementSteps.IsExistsLink("CardTariff");
            expectedCard.CardService = elementSteps.IsExistsLink("CardService");

            expectedCard.Equals(actualCard).Should().BeTrue();
        }

        [AllureStep("Проверка подробной информации о виртуальной карте")]
        public void CheckDetailedInfoAboutCardVirtual(DataTable dataTable, CardDetailedInformation actualCard)
        {
            DbHelper dbHelper = new DbHelper();
            CardDetailedInformation expectedCard = dbHelper.DataTableToCardVirtualDetailedInformation(dataTable);
            expectedCard.CardTariff = elementSteps.IsExistsLink("CardTariff");
            expectedCard.CardService = elementSteps.IsExistsLink("CardService");

            expectedCard.Equals(actualCard).Should().BeTrue();
        }

        [AllureStep("Создать юзера")]
        public void CreateNewUser(DbClient dbClient, RegistrationNewUser user)
        {
            ApiMethods apiMethods = new ApiMethods();
            var response = apiMethods.GetResponseFromPostNewUser(user);
        }

        [AllureStep("Удалить юзера")]
        public void DeleteUser(DbClient dbClient, RegistrationNewUser user)
        {
            PowerBank_AQA_ApiTesting.Hooks.DbQueries.DeleteUser(user, dbClient);
        }

        [AllureStep("Проверка условий кредита")]
        public void CheckLoansConditions(BrowserSteps browserSteps, string btnMore, string conditionsName)
        {
            elementSteps.ClickToWebElement(btnMore);
            browserSteps.UpdateCurrentPage("LoansPageDetailedInformation");
            elementSteps.ClickToWebElement(conditionsName);
            browserSteps.SwitchTo(1);
            browserSteps.GetCurrentUrl().Should().Contain("http://172.17.1.35:9090/").And.Contain(".pdf");
            browserSteps.CloseWebPage();
            browserSteps.SwitchTo(0);
            browserSteps.UpdateCurrentPage("LoansPageDetailedInformation");
            elementSteps.ClickToWebElement("Кнопка назад");
            browserSteps.UpdateCurrentPage("LoansPage");
        }

        [AllureStep("Получение детальной информации о кредите")]
        public void CheckLoansInformation(BrowserSteps browserSteps, string btnMore, BsonDocument document)
        {
            elementSteps.ClickToWebElement(btnMore);
            browserSteps.UpdateCurrentPage("LoansPageDetailedInformation");
            LoanDetailedInformation actualLoanInformation = new();

            actualLoanInformation.Name =
                elementSteps.GetText("Название кредита");
            actualLoanInformation.InterestRate =
                Convert.ToInt32(TrimString.GetNumbersInString(elementSteps.GetText("Ставка годовых значение")));
            actualLoanInformation.AmountMin =
                Convert.ToInt32(TrimString.GetNumbersInString(elementSteps.GetText("Минимальная сумма значение")));
            actualLoanInformation.AmountMax =
                Convert.ToInt32(TrimString.GetNumbersInString(elementSteps.GetText("Максимальная сумма значение")));
            actualLoanInformation.MinDurationMonths =
                Convert.ToInt32(TrimString.GetNumbersInString(elementSteps.GetText("Минимальный срок значение")));
            actualLoanInformation.MaxDurationMonths =
                Convert.ToInt32(TrimString.GetNumbersInString(elementSteps.GetText("Максимальный срок значение")));
            actualLoanInformation.IsRevocable =
                TrimString.ReturnBoolFromString(elementSteps.GetText("Досрочное погашение значение"));
            actualLoanInformation.IsGuarantee =
                TrimString.ReturnBoolFromString(elementSteps.GetText("Поручители значение"));

            switch (actualLoanInformation.Name)
            {
                case "Кредит «Классический»":
                    {
                        actualLoanInformation.Name = "Классический";
                        break;
                    }
                case "Кредит «Срочный»":
                    {
                        actualLoanInformation.Name = "Срочный";
                        break;
                    }
                case "Кредит «Покупка автомобиля»":
                    {
                        actualLoanInformation.Name = "Покупка автомобиля";
                        break;
                    }
            }
            MongoDbHelper mongoDbHelper = new MongoDbHelper();
            LoanDetailedInformation expectedLoanInformation = mongoDbHelper.ConvertBsonToListLoans(document);

            actualLoanInformation.Should().BeEquivalentTo(expectedLoanInformation);

            elementSteps.ClickToWebElement("Кнопка назад");
            browserSteps.UpdateCurrentPage("LoansPage");
        }

        [AllureStep("Перейти на депозиты")]
        public void GoToDepositPage(BrowserSteps browserSteps)
        {
            browserSteps.UpdateCurrentPage("LeftMenu");
            elementSteps.ClickToWebElement("Депозиты");
            browserSteps.UpdateCurrentPage("DepositPage");
        }

        [AllureStep("Проверка условий депозита")]
        public void CheckDepositConditions(BrowserSteps browserSteps, string cardName, string conditionsName)
        {
            elementSteps.ClickToWebElement(cardName);
            browserSteps.UpdateCurrentPage("OpenedDepositPage");
            elementSteps.ClickToWebElement(conditionsName);
            browserSteps.SwitchTo(1);
            browserSteps.GetCurrentUrl().Should().Contain(".pdf");
            browserSteps.CloseWebPage();
            browserSteps.SwitchTo(0);
            browserSteps.UpdateCurrentPage("OpenedDepositPage");
            elementSteps.ClickToWebElement("Кнопка назад");
            browserSteps.UpdateCurrentPage("DepositPage");
        }

        [AllureStep("Проверить юридический адрес и телефон")]
        public void CheckLegalAddressAndPhone()
        {
            elementSteps.IsWebElementDisplayed("Поле 'Юридический адрес и Контакты'").Should().BeTrue();
            elementSteps.IsWebElementDisplayed("'Юридический адрес и Контакты' - значения").Should().BeTrue();
            elementSteps.GetText("'Юридический адрес и Контакты' - значения").Should()
                .Contain("РФ, Москва, 147231, Васильевская ул., 4, стр.1").And
                .Contain("8 800 777 55 33, 8 800 675 01 45");
        }

        [AllureStep("Получение детальной информации о оформленном депозите")]
        public void CheckOpenedDepositInformation(string cardsName, BrowserSteps browserSteps)
        {
            elementSteps.ClickToWebElement(cardsName);
            browserSteps.UpdateCurrentPage("OpenedDepositPage");

            OpenedDepositInformation openedDepositInformation = new();

            openedDepositInformation.CurrencyCode = elementSteps.GetText("Валюта депозита");
            openedDepositInformation.OpenDate = elementSteps.GetText("Дата открытия");
            openedDepositInformation.CloseDate = elementSteps.GetText("Дата закрытия");
            openedDepositInformation.Validity = elementSteps.GetText("Срок действия");
            openedDepositInformation.DepositAmount =
                Convert.ToInt32(TrimString.GetNumbersInString(elementSteps.GetText("Сумма вклада")));
            openedDepositInformation.InterestRate = Convert.ToDouble(
                TrimString.GetTrimPercent(elementSteps.GetText("Процентная ставка")), CultureInfo.InvariantCulture);
            openedDepositInformation.CurrentState =
                TrimString.ReturnBoolFromString(elementSteps.GetText("Текущее состояние"));
            openedDepositInformation.AutoRenewal =
                TrimString.ReturnBoolFromString(elementSteps.GetText("Автопродление"));
            openedDepositInformation.Capitalization =
                TrimString.ReturnBoolFromString(elementSteps.GetText("Капитализация"));
            openedDepositInformation.Revocable =
                TrimString.ReturnBoolFromString(elementSteps.GetText("Досрочное закрытие"));

            elementSteps.ClickToWebElement("Кнопка назад");
            browserSteps.UpdateCurrentPage("DepositPage");
        }

        [AllureStep("Заполнение объектов карт данными")]
        public List<CardShortInformation> FillingShortCardInfo()
        {
            List<CardShortInformation> cards = new List<CardShortInformation>();

            List<string> cardsName = elementSteps.GetTextFromCollection("CardLabel");
            List<string> cardsType = elementSteps.GetTextFromCollection("CardType");
            List<string> cardsShortDescription = elementSteps.GetTextFromCollection("CardShortDescription");
            List<string> cardsServicePrice = elementSteps.GetTextFromCollection("CardServicePrice");
            List<string> cardsCashBack = elementSteps.GetTextFromCollection("CardCashback");

            for (int i = 0; i < cardsName.Count; i++)
            {
                CardShortInformation card = new CardShortInformation();
                card.CardName = cardsName[i];
                card.CardType = cardsType[i];
                card.CardShortDescription = cardsShortDescription[i];
                card.CardServicePrice = TrimString.GetNumbersInString(cardsServicePrice[i]);
                card.CardCashBack = TrimString.GetNumbersInString(cardsCashBack[i]);
                cards.Add(card);
            }

            return cards;
        }

        [AllureStep("Сравнение объектов карт")]
        public void CompareShortCardInfo(DataTable dataTable)
        {
            DbHelper dbHelper = new DbHelper();
            List<CardShortInformation> expectedCards =
                dbHelper.DataTableToListCardDebitAndVirtualShortInformation(dataTable);
            List<CardShortInformation> actualCards = FillingShortCardInfo();
            for (int i = 0; i < expectedCards.Count; i++)
            {
                expectedCards[i].Equals(actualCards[i]).Should().BeTrue();
            }
        }

        [AllureStep("Получение данных о курсах валюты со страницы")]
        public List<ExchangeRate> GetExchangeRates()
        {
            List<ExchangeRate> list = new List<ExchangeRate>();
            ExchangeRate exchangeRate = new ExchangeRate();

            exchangeRate.CurrencyCode = elementSteps.GetText("Код USD");
            exchangeRate.BuyingRate = Convert.ToDouble(elementSteps.GetText("Покупка USD"));
            exchangeRate.SellingRate = Convert.ToDouble(elementSteps.GetText("Продажа USD"));
            list.Add(exchangeRate);

            exchangeRate.CurrencyCode = elementSteps.GetText("Код EUR");
            exchangeRate.BuyingRate = Convert.ToDouble(elementSteps.GetText("Покупка EUR"));
            exchangeRate.SellingRate = Convert.ToDouble(elementSteps.GetText("Продажа EUR"));
            list.Add(exchangeRate);

            exchangeRate.CurrencyCode = elementSteps.GetText("Код CNY");
            exchangeRate.BuyingRate = Convert.ToDouble(elementSteps.GetText("Покупка CNY"));
            exchangeRate.SellingRate = Convert.ToDouble(elementSteps.GetText("Продажа CNY"));
            list.Add(exchangeRate);

            exchangeRate.CurrencyCode = elementSteps.GetText("Код CAD");
            exchangeRate.BuyingRate = Convert.ToDouble(elementSteps.GetText("Покупка CAD"));
            exchangeRate.SellingRate = Convert.ToDouble(elementSteps.GetText("Продажа CAD"));
            list.Add(exchangeRate);

            exchangeRate.CurrencyCode = elementSteps.GetText("Код GBP");
            exchangeRate.BuyingRate = Convert.ToDouble(elementSteps.GetText("Покупка GBP"));
            exchangeRate.SellingRate = Convert.ToDouble(elementSteps.GetText("Продажа GBP"));
            list.Add(exchangeRate);

            exchangeRate.CurrencyCode = elementSteps.GetText("Код JPY");
            exchangeRate.BuyingRate = Convert.ToDouble(elementSteps.GetText("Покупка JPY"));
            exchangeRate.SellingRate = Convert.ToDouble(elementSteps.GetText("Продажа JPY"));
            list.Add(exchangeRate);

            return list;
        }

        [AllureStep("Сравнение курсов валют")]
        public void CompareExchangeRates(List<ExchangeRate> expectedResults, List<ExchangeRate> actualResults)
        {
            foreach (var expectedResult in expectedResults)
            {
                foreach (var actualResult in actualResults)
                {
                    if (actualResult.CurrencyCode.Equals(expectedResult.CurrencyCode))
                    {
                        actualResult.SellingRate.Equals(expectedResult.SellingRate).Should().BeTrue();
                        actualResult.BuyingRate.Equals(expectedResult.BuyingRate).Should().BeTrue();
                    }
                }
            }
        }

        [AllureStep("Сравнение периода выписки")]
        public void CompareDateStatement(string locator)
        {
            elementSteps.ClickToWebElement(locator);
            string expectedResult = elementSteps.GetAttributeValue("Enter period", "value");
            elementSteps.ClickToWebElement("Submit");
            string actualResult = elementSteps.GetText("Period");
            actualResult.Contains(expectedResult).Should().BeTrue();
        }

        [AllureStep("Очистить поле даты и заполнить")]
        public void ClearDataFieldAndFill(string locator, string text)
        {
            for (int i = 0; i < 16; i++)
            {
                elementSteps.SendKeys(locator, Keys.Backspace);
            }

            elementSteps.SendKeys(locator, text);

        }

        [AllureStep("Выбор даты на текущей странице календаря")]
        public void ClickToCalendar(string startDate, string endDate)
        {
            elementSteps.ClickToWebElement("Calendar");
            elementSteps.ClickToWebElement(startDate);
            elementSteps.ClickToWebElement(endDate);
            elementSteps.ClickToWebElement("Accept calendar date");
        }

        [AllureStep("Выбор даты в другом месяце календаря")]
        public void ClickToCalendar(string shift, int numberOfShift, string startDate, string endDate)
        {
            elementSteps.ClickToWebElement("Calendar");
            for (int i = 0; i < numberOfShift; i++)
            {
                elementSteps.ClickToWebElement(shift);
            }

            elementSteps.ClickToWebElement(startDate);
            elementSteps.ClickToWebElement(endDate);
            elementSteps.ClickToWebElement("Accept calendar date");
        }

        [AllureStep("Сверка дат по radioButton")]
        public bool CheckDateRadioButton(string locatorRadioButton)
        {
            elementSteps.ClickToWebElement(locatorRadioButton);
            string expectedResult = elementSteps.GetAttributeValue("Enter period", "value");
            string startDate = DateTime.Today.AddMonths(-Convert.ToInt32(locatorRadioButton[0] - 48))
                .ToShortDateString();
            string endDate = DateTime.Today.AddDays(-1).ToShortDateString();
            string actualResult = startDate + " - " + endDate;
            return expectedResult.Equals(actualResult);
        }

        public TransferInformation GetInformationAfterTransfer(string cardLocator, string recipientCard, string amount, string cvv,
            string comment)
        {
            TransferInformation transferInformation = new TransferInformation();
            elementSteps.ClickToWebElement(cardLocator);
            elementSteps.ClickToWebElement("Оплатить и перевести");
            elementSteps.ClickToWebElement("Перевод на карту");
            elementSteps.SendKeys("Введите номер карты", recipientCard);
            elementSteps.ClickToWebElement("Продолжить");
            elementSteps.SendKeys("Введите сумму", amount);
            elementSteps.SendKeys("Введите код", cvv);
            elementSteps.SendKeys("Введите комментарий", comment);
            elementSteps.ClickToWebElement("Перевести");
            transferInformation.CardNumberRecipient = elementSteps.GetText("Актуальная карта получателя");
            transferInformation.CardNumberRecipient = transferInformation.CardNumberRecipient.Substring(transferInformation.CardNumberRecipient.Length - 4);
            transferInformation.CardNumberSender = elementSteps.GetText("Актуальная карта отправителя");
            transferInformation.CardNumberSender = transferInformation.CardNumberSender.Substring(transferInformation.CardNumberSender.Length - 4);
            transferInformation.Amount = elementSteps.GetText("Сумма перевода");
            transferInformation.Amount = Regex.Replace(transferInformation.Amount, "[А-Яа-я :\u20bd]", "");
            return transferInformation;
        }

        public TransferInformation GetInformationBeforeTransfer(string sender, string recipient, string amount)
        {
            TransferInformation transferInformation = new TransferInformation()
            {
                CardNumberSender = elementSteps.GetText(sender),
                CardNumberRecipient = recipient,
                Amount = amount
            };
            transferInformation.CardNumberSender = transferInformation.CardNumberSender.Substring(transferInformation.CardNumberSender.Length - 4);
            transferInformation.CardNumberRecipient = transferInformation.CardNumberRecipient.Substring(transferInformation.CardNumberRecipient.Length - 4);
            return transferInformation;
        }

        public void EnterCardNumberTransfer(string cardLocator, string recipientCard)
        {
            elementSteps.ClickToWebElement(cardLocator);
            elementSteps.ClickToWebElement("Оплатить и перевести");
            elementSteps.ClickToWebElement("Перевод на карту");
            elementSteps.SendKeys("Введите номер карты", recipientCard);
        }

        public void EnterAmountAndCvvTransfer(string amount, string cvv)
        {
            elementSteps.SendKeys("Введите сумму", amount);
            elementSteps.SendKeys("Введите код", cvv);
        }
    }
}