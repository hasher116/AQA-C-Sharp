using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;


namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "DepartmentsPage", Url = "departments")]
    public class DepartmentsPage : Page
    {
        [Element(Name = "Карта", Locator = "//div[contains(@class, 'leaflet-container')]")]
        Button elementMap;

        [Element(Name = "Увеличение масштаба", Locator = "//a[@class='leaflet-control-zoom-in']")]
        Button buttonZoomOut;

        [Element(Name = "Масштаб карты", Locator = "//div[contains(@class, 'leaflet-proxy leaflet-zoom-animated')]")]
        Element elementScale;

        [Element(Name = "Куки окно", Locator = "//p[contains(text(), 'Power Bank')]/..")]
        Block elementCookiesWindow;

        [Element(Name = "Куки закрыть", Locator = "//button[text()='Закрыть']")]
        Button buttonCookiesClose;

        [Element(Name = "Куки условия", Locator = "//a[text()='Условиями обработки персональных данных']")]
        Button elementCookiesTerms;

        [Element(Name = "Куки данные", Locator = "//a[text()='Cookies.']")]
        Button elementCookiesData;

        [Element(Name = "Иконка поиска", Locator = "//div[contains(@class, 'MuiFormControl')]//*[local-name()='svg']")]
        Element elementLoupeIcon;

        [Element(Name = "Плейсхолдер поиска", Locator = "//div[contains(@class, 'MuiFormControl')]//input[@placeholder='Поиск по адресу']")]
        Input inputSearchPlaceholder;

        [Element(Name = "Пустое поле поиска", Locator = "//div[contains(@class, 'MuiAutocomplete')]//input[@value='']")]
        Element elementSearchEmpty;

        [Element(Name = "Ввод \"при\"", Locator = "//div[contains(@class, 'MuiAutocomplete')]//input[@value='при']")]
        Element elementSearchPri;

        [Element(Name = "Очистить поиск", Locator = "//button[@data-test-id='clearButton']")]
        Button buttonClearSearch;

        [Element(Name = "Выбор", Locator = "//div[@role='presentation']")]
        Button buttonChoiceOdintsovo;

        [Element(Name = "Маркер Одинцово", Locator = "//img[@alt='Marker']")]
        Element elementMarkerOdintsovo;
    }
}
