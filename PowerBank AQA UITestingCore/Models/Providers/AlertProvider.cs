using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Exeptions;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;
using System;

namespace PowerBank_AQA_UITestingCore.Models.Providers
{
    public class AlertProvider : IAlertProvider
    {
        public IAlert Alert { get; init; }

        public string Text => Alert.Text;

        public void SendAccept()
        {
            try
            {
                Alert.Accept();
            }
            catch (Exception ex)
            {

                throw new AlertException($"При нажатии Приянть в Alert окне возникла ошибка {ex.Message}");
            }
        }

        public void SendDissmiss()
        {
            try
            {
                Alert.Dismiss();
            }
            catch (Exception ex)
            {

                throw new AlertException($"При нажатии Отклонить в Alert окне возникла ошибка {ex.Message}");
            }
        }

        public void SendKeys(string keys)
        {
            try
            {
                Alert.SendKeys(keys);
            }
            catch (Exception ex)
            {

                throw new AlertException($"При попытке ввести текст {keys} в Alert окне возникла ошибка {ex.Message}");
            }
        }
    }
}
