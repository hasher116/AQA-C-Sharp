using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName ="Main", Url = "https://www.onliner.by/")]
    public class OnePage : Page
    {
        [Element(Name ="Каталог", Locator = "//span[@class='b-main-navigation__text'][1]")]
        Button catalog;

        [Element(Name = "Меню", Locator = "//a[@class='header-style__underlay']")]
        Button menu;

        [Element(Name = "Смартфоны", Locator = "//span[@class = 'project-navigation__sign' and text() = 'Смартфоны']")]
        Button smartphones;
    }
}
