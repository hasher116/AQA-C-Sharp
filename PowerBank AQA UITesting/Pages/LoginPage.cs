using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;

namespace PowerBank_AQA_UITesting.Pages
{
    [Page(PageName = "LoginPage", Url = "login")]
    public class LoginPage : Page
    {
        [Element(Name = "Вход по телефону", Locator = "//button[text()='По телефону']")]
        Button loginWithTelephoneNumber;

        [Element(Name = "Вход по паспорту", Locator = "//button[text()='По паспорту']")]
        Button loginWithPassport;

        [Element(Name = "Поле ввода для телефона", Locator = "//input[@name='mobilePhone']")]
        Input MobilePhone;

        [Element(Name = "Поле ввода для паспорта", Locator = "//input[@name='passportNumber']")]
        Input PassportNumber;

        [Element(Name = "Поле ввода для пароля", Locator = "//input[@name='passwordEncode']")]
        Input PasswordEncode;

        [Element(Name = "Войти", Locator = "//button[text()='Войти']")]
        Button submitButton;

        [Element(Name = "Регистрация", Locator = "//button[text()='Зарегистрироваться']")]
        Button registration;

        [Element(Name = "Забыли пароль", Locator = "//a[@href='/recovery']")]
        Button forgetPassword;

        [Element(Name = "Банкоматы и отделения", Locator = "//a[contains(@class, 'MuiTypography') and @href='/departments']")]
        Button buttonDepartment;
    }
}
