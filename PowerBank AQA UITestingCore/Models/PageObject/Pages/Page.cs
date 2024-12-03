using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.PageObject.Attributes;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;
using System.Reflection;
using PowerBank_AQA_UITestingCore.Models.PageObject.Frames;
using PowerBank_AQA_UITestingCore.Extensions;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Pages
{
    public class Page : BasePage
    {
        public override string Name => GetType().GetCustomAttribute<PageAttribute>()?.PageName;

        public override string Url => GetType().GetCustomAttribute<PageAttribute>()?.Url;

        public override Node Root { get; set; }

        public void SetProvider(IDriverProvider provider, Settings.Settings settings)
        {
            DriverProvider = provider;
            Settings = settings;
        }

        public override Block GetBlock(string name)
        {
            var root = Local ?? Root;
            var block = root.SearchElementBy(name, ObjectType.Block);
            (block.Object as Block)?.SetProvider(DriverProvider, Settings);
            ((Block)block.Object).Root = block;
            Local = block;
            return (Block)block.Object;
        }

        public override IElement GetElement(string name)
        {
            var root = Local ?? Root;
            var element = root.SearchElementBy(name);
            ((IElement)element.Object).Root = element;
            ((IElement)element.Object).SetProvider(DriverProvider, Settings);
            return (IElement)element.Object;
        }

        public override IEnumerable<IElement> GetCollection(string name)
        {
            var root = Local ?? Root;
            var collection = root.SearchCollectionBy(name);
            var elements = DriverProvider.GetElements(
            ((IElement)collection.Object).Locator,
            ((IElement)collection.Object).How);
            var lst = new List<IElement>();

            foreach (var element in elements)
            {
                var obj = (IElement)((IElement)collection.Object).Clone();
                obj.Root = collection;
                obj.SetProvider(DriverProvider, Settings);
                obj.ElementProvider = element;
                lst.Add(obj);
            }

            return lst;
        }

        public override IEnumerable<string> GetPrimaryElements()
        {
            var elements = Root.Childrens.Where(c => ((IElement)c.Object).Optional == false);
            return elements.Select(element => element.Name).ToList();
        }

        #region Работа с фреймами

        public override IPage GetDefaultFrame()
        {
            var frame = Local.SearchNearestFrame();
            (frame.Object as Frame)?.SetProvider(DriverProvider, Settings, true);
            DriverProvider = (frame.Object as Frame)?.Default();
            Local = null;
            return this;
        }

        public override Frame GetParentFrame()
        {
            throw new System.NotImplementedException();
        }

        public override Frame GetFrame(string name)
        {
            var root = Local ?? Root;
            var frame = root.SearchElementBy(name, ObjectType.Frame);
            (frame.Object as Frame)?.SetProvider(DriverProvider, Settings);
            ((Frame)frame.Object).Root = frame;
            Local = frame;
            return frame.Object as Frame;
        }

        #endregion

        public override void GoToPage()
        {
            DriverProvider.GoToUrl(Url);
        }

        public override void PageTop()
        {
            var action = new Actions(DriverProvider.GetDriver());
            action.SendKeys(Keys.Control).SendKeys(Keys.Home).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }

        public override void PageDown()
        {
            var action = new Actions(DriverProvider.GetDriver());
            action.SendKeys(Keys.Control).SendKeys(Keys.End).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }
    }
}
