using Bogus.DataSets;
using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Extensions;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.Mediator;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public class Element : IElement, ICloneable
    {
        public Element(string name, string locator, bool optional = false)
        {
            Name = name;
            Locator = locator;
            Optional = optional;
        }

        public Element(How how, string locator)
        {
            Locator = locator;
            How = how;
        }

        public IDriverProvider Driver { get; set; } = default;

        public IElementProvider ElementProvider { get; set; } = default;

        public Settings.Settings Settings { get; set; }

        public Node Root { get; set; }

        public string Name { get; set; }

        public How How { get; set; } = How.XPath;

        public string Locator { get; set; }

        public By By => How.GetBy(Locator);

        public bool Optional { get; set; }

        public string Text => Mediator.Execute(() => ElementProvider.Text) as string;

        public string Tag => Mediator.Execute(() => ElementProvider.Tag) as string;

        public object Value => Mediator.Execute(() => GetAttribute("value"));

        public bool Loaded => (bool)Mediator.Execute(() => ElementProvider.Loaded);

        public bool NotLoaded => (bool)Mediator.Execute(() => ElementProvider.NotLoaded);

        public bool Enabled => (bool)Mediator.Execute(() => ElementProvider.Enabled);

        public bool Disabled => (bool)Mediator.Execute(() => ElementProvider.Disabled);

        public bool Displayed => (bool)Mediator.Execute(() => ElementProvider.Displayed);

        public bool NotDisplayed => (bool)Mediator.Execute(() => ElementProvider.NotDisplayed);

        public bool Selected => (bool)Mediator.Execute(() => ElementProvider.Selected);

        public bool NotSelected => (bool)Mediator.Execute(() => ElementProvider.NotSelected);

        public bool Editable => (bool)Mediator.Execute(() => ElementProvider.Editable);

        protected IMediator Mediator { get; set; }

        public string ReplacedLocator { get; set; }

        public void SetProvider(IDriverProvider provider, Settings.Settings settings)
        {
            Settings = settings;
            Driver = provider;
            Mediator = new ElementMediator(settings.Timeout, provider.GetDriver());
            ElementProvider = GetElementBy(Locator);
        }

        public void Get()
        {
            ElementProvider = Mediator.Execute(() => Driver.GetElement(Locator, How)) as IElementProvider;
        }

        public void GetParent()
        {

        }

        Settings.Settings IElement.Settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IElement Find(Node element, How how = How.XPath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IElement> FindAll(Node element, How how = How.XPath)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            Mediator.Execute(() => ElementProvider.Clear());
        }

        public string GetAttribute(string name)
        {
            return Mediator.Wait(() => ElementProvider.GetAttribute(name)) as string;
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void PressKeys(string keys)
        {
            throw new NotImplementedException();
        }

        public bool IsTextContains(string text)
        {
            return (bool)Mediator.Wait(() => ElementProvider.TextContain(text));
        }

        public bool IsTextEquals(string text)
        {
            return (bool)Mediator.Wait(() => ElementProvider.TextEqual(text));
        }

        public bool IsTextMatch(string text)
        {
            return (bool)Mediator.Wait(() => ElementProvider.TextMatch(text));

        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        private IElementProvider GetElementBy(string locator, How how = How.XPath)
        {
            return Mediator.Execute(() => Driver.GetElement(locator, how)) as IElementProvider;
        }
    }
}
