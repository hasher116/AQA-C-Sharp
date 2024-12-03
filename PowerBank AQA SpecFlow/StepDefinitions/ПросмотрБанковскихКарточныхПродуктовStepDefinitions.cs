using Bogus.DataSets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_SpecFlow.Dto;
using PowerBank_AQA_SpecFlow.Support;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Helpers;
using System;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace PowerBank_AQA_SpecFlow.StepDefinitions
{
    [Binding]
    public class ПросмотрБанковскихКарточныхПродуктовStepDefinitions
    {
        private BrowserStepDefinitions browserStepDefinitions;
        private ElementStepDefinitions elementStepDefinitions;
        private IConfiguration configuration;
        private DataBaseStepDefinitions dataBaseStepDefinitions;
        private DbClient dbCard;

        public ПросмотрБанковскихКарточныхПродуктовStepDefinitions(BrowserStepDefinitions browserStepDefinitions,
            ElementStepDefinitions elementStepDefinitions,
            DataBaseStepDefinitions dataBaseStepDefinitions)
        {
            this.browserStepDefinitions = browserStepDefinitions;
            this.elementStepDefinitions = elementStepDefinitions;
            this.dataBaseStepDefinitions = dataBaseStepDefinitions;
            configuration = Configuration.GetConfiguration();
            dbCard = new DbClient(configuration, "CardDb");
            dbCard.Create();
        }

        [Given(@"Открыта страница с карточными продуктами")]
        public void GivenОткрытаСтраницаСКарточнымиПродуктами()
        {
            browserStepDefinitions.SetCurrentPage("LoginPage");
            elementStepDefinitions.SendKeys("Поле ввода для телефона", "9772345685");
            elementStepDefinitions.SendKeys("Поле ввода для пароля", "ls23Ghq#wEr");
            elementStepDefinitions.ClickToWebElement("Войти");
            browserStepDefinitions.UpdateCurrentPage("LeftMenu");
            elementStepDefinitions.ClickToWebElement("Карты");
            browserStepDefinitions.SetCurrentPage("CardPage");
        }

        [When(@"Перешёл на (.*) карты")]
        public void WhenПерешёлНаКарты(string type)
        {
            elementStepDefinitions.ClickToWebElement(type);
        }

        [Then(@"Произошло переключение на (.*) карты")]
        public void ThenПроизошлоПереключениеНаКарты(string type)
        {
            elementStepDefinitions.IsWebElementEnabled(type).Should().BeTrue();
        }

        [Given(@"Открыты (.*) карточные продукты")]
        public void GivenОткрытаСтраницаСОпределённымТипомКарты(string type)
        {
            GivenОткрытаСтраницаСКарточнымиПродуктами();
            elementStepDefinitions.ClickToWebElement(type);
        }

        [When(@"Нажал на (.*)")]
        public void WhenНажалНаКарту(string card)
        {
            elementStepDefinitions.ClickToWebElement(card);
            browserStepDefinitions.UpdateCurrentPage("DetailedCardPage");
        }

        [Then(@"Открывается страница с (.*)")]
        public void ThenОткрываетсяСтраницаСКартой(string card)
        {
            elementStepDefinitions.GetText("CardLabel").Should().BeEquivalentTo(card);
        }

        [Then(@"Являющаяся (.*)")]
        public void ThenЯвляющаясяТипом(string type)
        {
            elementStepDefinitions.GetText("CardType").Should().BeEquivalentTo(type);
        }

        [Then(@"(.*), (.*) сопоставимы с базой данных")]
        public void ThenТипИКартаСопоставимыСБазойДанных(string type, string card)
        {
            switch (type)
            {
                case "Дебетовая карта":
                    {
                        CardDetailedInformation actualCardDetailedInformation = new CardDetailedInformation()
                        {
                            CardName = elementStepDefinitions.GetText("CardLabel"),
                            CardType = elementStepDefinitions.GetText("CardType"),
                            CardCashBack = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardCashBack")),
                            CardServicePrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardServicePrice")),
                            CardCashMaxDay = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardCashMaxDay")),
                            CardCashMaxMonth = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardCashMaxMonth")),
                            CardNotificationPrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardNotificationPrice")),
                            CardPaymentSystem = elementStepDefinitions.GetText("CardPaymentSystem").ToLowerInvariant()
                        };

                        var cardTable = DbQueries.GetCardInformation(dbCard, actualCardDetailedInformation.CardName);
                        var expectedCardDetailedInformation = dataBaseStepDefinitions.GetDetailedInfoAboutDebitCard(cardTable);
                        card.Should().BeEquivalentTo(actualCardDetailedInformation.CardName);
                        actualCardDetailedInformation.Should().BeEquivalentTo(expectedCardDetailedInformation);
                        break;
                    }
                case "Кредитная карта":
                    {
                        CardDetailedInformation actualCardDetailedInformation = new CardDetailedInformation()
                        {
                            CardName = elementStepDefinitions.GetText("CardLabel"),
                            CardType = elementStepDefinitions.GetText("CardType"),
                            CardPaymentSystem = elementStepDefinitions.GetText("CardPaymentSystem").ToLowerInvariant(),
                            CardInterestRate = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardInterestRate")),
                            CardInterestFreeDays = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardInterestFreeDays")),
                            CardAmountCreditMax = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardAmountCreditMax")),
                            CardServicePrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardServicePrice")),
                            CardCashMaxDay = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardCashMaxDay")),
                            CardNotificationPrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardNotificationPrice")),
                            CardCashWithdrawalFee = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardCashWithdrawalFee"))
                        };
                        var cardTable = DbQueries.GetCardInformation(dbCard, actualCardDetailedInformation.CardName);
                        var expectedCardDetailedInformation = dataBaseStepDefinitions.GetDetailedInfoAboutCreditCard(cardTable);
                        card.Should().BeEquivalentTo(actualCardDetailedInformation.CardName);
                        actualCardDetailedInformation.Should().BeEquivalentTo(expectedCardDetailedInformation);
                        break;
                    }
                case "Виртуальная карта":
                    {
                        CardDetailedInformation actualCardDetailedInformation = new CardDetailedInformation()
                        {
                            CardName = elementStepDefinitions.GetText("CardLabel"),
                            CardType = elementStepDefinitions.GetText("CardType"),
                            CardPaymentSystem = elementStepDefinitions.GetText("CardPaymentSystem").ToLowerInvariant(),
                            CardCashBack = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardCashBack")),
                            CardServicePrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardServicePrice")),
                            CardNotificationPrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("CardNotificationPrice"))

                        };
                        var cardTable = DbQueries.GetCardInformation(dbCard, actualCardDetailedInformation.CardName);
                        var expectedCardDetailedInformation = dataBaseStepDefinitions.GetDetailedInfoAboutVirtualCard(cardTable);
                        card.Should().BeEquivalentTo(actualCardDetailedInformation.CardName);
                        actualCardDetailedInformation.Should().BeEquivalentTo(expectedCardDetailedInformation);
                        break;
                    }
            }
        }

        [Then(@"Краткая информация (.*), (.*) соответсвует БД")]
        public void ThenКраткаяИнформацияТипаИКартыСоответствуетБД(string type, string card)
        {
            switch (card)
            {
                case "Game Card":
                    {
                        CardShortInformation actualCardShortInformation = new CardShortInformation()
                        {

                            CardName = elementStepDefinitions.GetText("Game Card"),
                            CardType = elementStepDefinitions.GetText("Тип Game Card"),
                            CardShortDescription = elementStepDefinitions.GetText("Описание Game Card"),
                            CardCashBack = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("Кешбэк Game Card")),
                            CardServicePrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("Обслуживание Game Card"))
                        };

                        var cardTable = DbQueries.GetCardInformation(dbCard, actualCardShortInformation.CardName);
                        var expectedCardShortInformation = dataBaseStepDefinitions.GetShortInfoAboutDebitCard(cardTable);
                        type.Should().BeEquivalentTo(actualCardShortInformation.CardType);
                        actualCardShortInformation.Should().BeEquivalentTo(expectedCardShortInformation);
                        break;
                    }
                case "Cache Card":
                    {
                        CardShortInformation actualCardShortInformation = new CardShortInformation()
                        {

                            CardName = elementStepDefinitions.GetText("Лэйбл Cache Card"),
                            CardType = elementStepDefinitions.GetText("Тип Cache Card"),
                            CardShortDescription = elementStepDefinitions.GetText("Описание Cache Card"),
                            CardInterestRate = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("Процент годовых Cache Card")),
                            CardInterestFreeDays = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("Беспроцентный период Cache Card")),
                            CardServicePrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("Стоимость обслуживания Cache Card"))
                        };

                        var cardTable = DbQueries.GetCardInformation(dbCard, actualCardShortInformation.CardName);
                        var expectedCardShortInformation = dataBaseStepDefinitions.GetShortInfoAboutCreditCard(cardTable);
                        type.Should().BeEquivalentTo(actualCardShortInformation.CardType);
                        actualCardShortInformation.Should().BeEquivalentTo(expectedCardShortInformation);
                        break;
                    }
                case "Power Card":
                    {
                        CardShortInformation actualCardShortInformation = new CardShortInformation()
                        {

                            CardName = elementStepDefinitions.GetText("Power Card"),
                            CardType = elementStepDefinitions.GetText("Тип Power Card"),
                            CardShortDescription = elementStepDefinitions.GetText("Описание Power Card"),
                            CardCashBack = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("Кешбэк Power Card")),
                            CardServicePrice = RegexTemplate.GetNumberSelection(elementStepDefinitions.GetText("Обслуживание Power Card"))
                        };

                        var cardTable = DbQueries.GetCardInformation(dbCard, actualCardShortInformation.CardName);
                        var expectedCardShortInformation = dataBaseStepDefinitions.GetShortInfoAboutDebitCard(cardTable);
                        type.Should().BeEquivalentTo(actualCardShortInformation.CardType);
                        actualCardShortInformation.Should().BeEquivalentTo(expectedCardShortInformation);
                        break;
                    }
            }
        }

        [When(@"Нажал кнопку назад")]
        public void WhenНажалКнопкуНазад()
        {
            elementStepDefinitions.ClickToWebElement("Назад");
            browserStepDefinitions.UpdateCurrentPage("CardPage");
        }

        [Then(@"Возвращается назад на общую страницу карт с нажатой кнопкой (.*)")]
        public void ThenОткрываетсяСтраницаСНажатойКнопкой(string type)
        {
            elementStepDefinitions.IsWebElementEnabled(type).Should().BeTrue();
        }

        [When(@"Выбрал тарифы по карте")]
        public void WhenВыбралТарифыПоКарте()
        {
            elementStepDefinitions.ClickToWebElement("CardTariff");
        }

        [Then(@"Открывается PDF-файл с тарифом (.*)")]
        public void ThenОткрываетсяPDFФайлСТарифом(string card)
        {
            browserStepDefinitions.SwitchTo(1);
            browserStepDefinitions.GetCurrentUrl().Should().Contain(RegexTemplate.GetStringWithoutSpace(card + ".pdf"));
        }

        [When(@"Закрыл вкладку")]
        public void WhenЗакрылВкладку()
        {
            browserStepDefinitions.Close();
            browserStepDefinitions.SwitchTo(0);
        }

        [When(@"Выбрал условия выпуска")]
        public void WhenВыбралУсловияВыпуска()
        {
            elementStepDefinitions.ClickToWebElement("CardService");
        }

        [Then(@"Открывается PDF-файл с условиями выпуска")]
        public void ThenОткрываетсяPDFФайлСУсловиями()
        {
            browserStepDefinitions.SwitchTo(1);
            browserStepDefinitions.GetCurrentUrl().Should().Contain("UsingConditions.pdf");
        }
    }
}
