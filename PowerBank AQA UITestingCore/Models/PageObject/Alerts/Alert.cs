using PowerBank_AQA_UITestingCore.Models.Mediator;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Alerts
{
    public class Alert : IAlert
    {
        private IAlertProvider _alertProvider = null;

        public Alert(IDriverProvider provider, Settings.Settings settings)
        {
            var mediator = new AsyncLocal<IMediator>
            {
                Value = new AlertMediator(settings.Timeout, provider.GetDriver())
            };

            _alertProvider = (IAlertProvider)mediator.Value.Wait(provider.GetAlert);
        }

        public string Text => _alertProvider.Text;

        public void Accept()
        {
            _alertProvider.SendAccept();
        }

        public void Dissmiss()
        {
            _alertProvider.SendDissmiss();
        }

        public void SendKeys(string keeys)
        {
            _alertProvider.SendKeys(keeys);
        }

    }
}
