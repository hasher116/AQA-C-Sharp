using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Extensions;
using PowerBank_AQA_UITestingCore.Models.Mediator;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Frames
{
    public class Frame : Element
    {
        private string _frameName;
        private int? _number;

        public Frame(string name, string frameName, int number, string locator, bool optional = false) 
            : base(name, locator, optional)
        {
            _frameName = frameName;
            _number = number;
        }

        protected IMediator FrameMediator { get; set; }

        public void SetProvider(IDriverProvider provider, Settings.Settings settings, bool withoutParams = false)
        {
            Settings = settings;
            ElementProvider = null;
            FrameMediator = new FrameMediator(settings.Timeout, provider.GetDriver());
            Driver = GetFrame(provider, withoutParams);
        }

        public IDriverProvider Parent()
        {
            return FrameMediator.Execute(() => Driver.GetParentFrame()) as IDriverProvider;
        }

        public IDriverProvider Default()
        {
            return FrameMediator.Execute(() => Driver.GetDefaultFrame()) as IDriverProvider;
        }

        public Block GetBlock(string name)
        {
            var block = Root.SearchElementBy(name, Infrastructures.ObjectType.Block);

            (block.Object as Block)?.SetProvider(Driver, Settings);
            ((Block)block.Object).Root = block;
            return block.Object as Block;
        }

        public Frame GetFrame(string name)
        {
            var block = Root.SearchElementBy(name, Infrastructures.ObjectType.Frame);

            (block.Object as Frame)?.SetProvider(Driver, Settings);
            ((Frame)block.Object).Root = block;
            return block.Object as Frame;
        }

        public Element GetElement(string name)
        {
            var block = Root.SearchElementBy(name, Infrastructures.ObjectType.Element);

            (block.Object as Element)?.SetProvider(Driver, Settings);
            ((Element)block.Object).Root = block;
            return block.Object as Element;
        }

        private IDriverProvider GetFrame(IDriverProvider provider, bool withoutParams = false)
        {
            IDriverProvider _driver;

            if(withoutParams)
            {
                return provider;
            }

            if(_frameName != null)
            {
                _driver = FrameMediator.Execute(() => provider.GetFrame(_frameName)) as IDriverProvider;
            }

            if (_number != null)
            {
                _driver = FrameMediator.Execute(() => provider.GetFrame((int)_number)) as IDriverProvider;
            }

            _driver = FrameMediator.Execute(() => provider.GetFrame(By.XPath(Locator))) as IDriverProvider;
            return _driver;
        }
    }
}
