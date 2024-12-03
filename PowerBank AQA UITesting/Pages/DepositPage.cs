using PowerBank_AQA_UITesting.Pages.Blocks;
using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "DepositPage", Url = "/account/deposits")]
    public class DepositPage : Page
    {
        [Element(Name = "МоиДепозиты", Locator = "//span[text()='Мои депозиты']")]
        Element labelMyDeposits;

        [Element(Name = "ДепозитныеПродукты", Locator = "//h3[text()='Депозитные продукты']")]
        Element labelDepositProducts;

        [Element(Name = "ДепозитныеПродуктыRUB", Locator = "//button[@id='simple-tab-0']")]
        Button rub;

        [Element(Name = "ДепозитныеПродуктыUSD", Locator = "//button[@id='simple-tab-1']")]
        Button usd;

        [Element(Name = "ДепозитныеПродуктыEUR", Locator = "//button[@id='simple-tab-2']")]
        Button eur;

        [Element(Name = "КарточкаSpringOfferRUB", Locator = "//p[text()='SpringOffer']/../../div[1]")]
        Button springOfferDepositCard;

        [Element(Name = "LabelSpringOffer", Locator = "//p[text()='Депозит']/following-sibling::p[text()='SpringOffer']")]
        Element labelSpringOffer;

        [Element(Name = "КарточкаWinterOfferRUB", Locator = "//p[text()='WinterOffer']/../../div[1]")]
        Button winterOfferDepositCard;

        [Element(Name = "LabelWinterOffer", Locator = "//p[text()='Депозит']/following-sibling::p[text()='WinterOffer']")]
        Element labelWinterOffer;

        [Element(Name = "КарточкаSummerOfferRUB", Locator = "//p[text()='SummerOffer']/../../div[1]")]
        Button summerOfferDepositCard;

        [Element(Name = "LabelSummerOffer", Locator = "//p[text()='Депозит']/following-sibling::p[text()='SummerOffer']")]
        Element labelSummerOffer;

        [Element(Name = "КарточкаUniversalUSD", Locator = "//p[text()='Universal']/../../div[1]")]
        Button usdUniversalDepositCard;

        [Element(Name = "LabelUniversal", Locator = "//p[text()='Депозит']/following-sibling::p[text()='Universal']")]
        Element labelUniversal;

        [Element(Name = "КарточкаUSDKeepOfferDepositCard", Locator = "//p[text()='UsdKeepOffer']/../../div[1]")]
        Button usdKeepOfferDepositCard;

        [Element(Name = "LabelKeepOffer", Locator = "//p[text()='Депозит']/following-sibling::p[text()='UsdKeepOffer']")]
        Element labelKeepOffer;

        [Element(Name = "КарточкаGarantDepositCardUSD", Locator = "//p[text()='Garant']/../../div[1]")]
        Button garantOfferDepositCard;

        [Element(Name = "LabelGarant", Locator = "//p[text()='Депозит']/following-sibling::p[text()='Garant']")]
        Element labelGarant;

        [Element(Name = "КарточкаKeepOfferEUR", Locator = "//p[text()='EuroKeepOffer']/../../div[1]")]
        Button euroKeepOfferDepositCard;

        [Element(Name = "LabelEuroKeepOffer", Locator = "//p[text()='Депозит']/following-sibling::p[text()='EuroKeepOffer']")]
        Element labelEuroKeepOffer;

        [Element(Name = "КарточкаClassicEUR", Locator = "//p[text()='Classic']/../../div[1]")]
        Button eurClassicDepositCard;

        [Element(Name = "LabelClassic", Locator = "//p[text()='Депозит']/following-sibling::p[text()='Classic']")]
        Element labelClassic;

        [Element(Name = "КарточкаClassicPlusEUR", Locator = "//p[text()='ClassicPlus']/../../div[1]")]
        Button eurClassicPlucDepositCard;

        [Element(Name = "LabelClassic+", Locator = "//p[text()='Депозит']/following-sibling::p[text()='ClassicPlus']")]
        Element labelClassicPlus;

        [Element(Name = "Отозвать депозит", Locator = "//button[text()='Отозвать депозит']")]
        Button withdrawDeposit;

        //[Element(Name = "Сумма на депозите", Locator = "//*[@class = 'MuiTypography-root MuiTypography-body1 css-d462g2']")]
        //Element amountOfMoneyOnDeposit;

        [Element(Name = "КарточкаSummerOfferRUBUserHave", Locator = "//span[text()='Мои депозиты']/../../following-sibling::div/div/div/p[text()='SummerOffer']/../..")]
        Button summerOfferDepositCardUserHave;

        [Element(Name = "КарточкаWinterOfferRUBUserHave", Locator = "//span[text()='Мои депозиты']/../../following-sibling::div/div/div/p[text()='WinterOffer']/../..")]
        Button winterOfferDepositCardUserHave;
        
        [Element(Name = "'Юридический адрес и Контакты' - значения", Locator = "//p[text()='Юридический адрес:']/../following-sibling::div")]
        Element AdressContactsValueLabel;

        [Element(Name = "Поле 'Юридический адрес и Контакты'", Locator = "//p[text()='Юридический адрес:']")]
        Element AdressContactsLabel;
    }
}
