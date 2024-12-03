using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public interface IElement : IEntity
    {
        IDriverProvider Driver { get; set; }

        IElementProvider ElementProvider { get; set; }

        Settings.Settings Settings { get; set; }

        string Text { get; }

        string Tag { get; }

        object Value { get; }

        bool Loaded { get; }

        bool NotLoaded { get; }

        bool Enabled { get; }

        bool Disabled { get; }

        bool Displayed { get; }

        bool NotDisplayed { get; }

        bool Selected { get; }

        bool NotSelected { get; }

        bool Editable { get; }

        void SetProvider(IDriverProvider provider, Settings.Settings settings);

        void Get();

        void GetParent();

        IElement Find(Node element, How how = How.XPath);

        IEnumerable<IElement> FindAll(Node element, How how = How.XPath);

        void Clear();

        string GetAttribute(string name);

        void Move();

        void PressKeys(string keys);

        bool IsTextContains(string text);

        bool IsTextEquals(string text);

        bool IsTextMatch(string text);

        object Clone();
    }
}
