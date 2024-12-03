namespace PowerBank_AQA_UITestingCore.Models.Providers.Interfaces

{
    public interface IAlertProvider
    {
        string Text { get; }

        void SendAccept();

        void SendDissmiss();

        void SendKeys(string keys);
    }
}
