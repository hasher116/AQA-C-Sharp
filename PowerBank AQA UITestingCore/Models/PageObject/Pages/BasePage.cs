using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Frames;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Pages
{
    public abstract class BasePage : IPage
    {
        private IDriverProvider _driverProvider;

        public IDriverProvider DriverProvider
        {
            get => _driverProvider;
            set => _driverProvider = value;
        }

        public abstract string Url { get; }

        public Settings.Settings Settings { get; set; }

        public abstract string Name { get; }

        public abstract Node Root { get; set; }

        public virtual Node Local { get; set; } = null;

        public abstract Block GetBlock(string name);

        public void BackToPage() => Local = null;

        public abstract IElement GetElement(string name);

        public abstract IEnumerable<IElement> GetCollection(string name);

        public abstract IEnumerable<string> GetPrimaryElements();

        public abstract void GoToPage();

        public abstract void PageTop();

        public abstract void PageDown();

        public bool IsLoadElements()
        {
            var errors = new List<string>();
            var elementsNames = GetPrimaryElements();

            (elementsNames as List<string>)?.ForEach(name =>
            {
                var element = GetElement(name);
                if (!element.Loaded)
                {
                    errors.Add(name);
                }
            });

            if (!errors.Any())
            {
                return true;
            }

            var aggregate = string.Join(", ", errors);
            Log.Logger().LogError($"element/s \"{aggregate}\" not initialize on page \"{Name}\"");
            return false;
        }

        public abstract IPage GetDefaultFrame();

        public abstract Frame GetParentFrame();

        public abstract Frame GetFrame(string name);
    }
}
