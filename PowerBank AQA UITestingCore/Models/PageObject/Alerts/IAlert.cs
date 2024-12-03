namespace PowerBank_AQA_UITestingCore.Models.PageObject.Alerts
{
    public interface IAlert
    {
        string Text { get; }

        void Accept();

        void Dissmiss();

        void SendKeys(string keys);
    }
}
