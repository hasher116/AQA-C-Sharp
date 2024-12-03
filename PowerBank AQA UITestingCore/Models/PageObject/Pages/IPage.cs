using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.PageObject.Frames;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Pages
{
    public interface IPage
    {
        Settings.Settings Settings { get; set; }

        string Name { get; }

        string Url { get; }

        Node Root { get; set; }

        Node Local { get; set; }

        Block GetBlock(string name);

        void BackToPage();

        IElement GetElement(string name);

        IEnumerable<IElement> GetCollection(string name);

        IEnumerable<string> GetPrimaryElements();

        IPage GetDefaultFrame();

        Frame GetParentFrame();

        Frame GetFrame(string name);

        void GoToPage();

        void PageTop();

        void PageDown();

        bool IsLoadElements();
    }
}
