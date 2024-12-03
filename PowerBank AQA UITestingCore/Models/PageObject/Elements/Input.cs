namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public class Input : Element
    {
        public Input(string name, string locator, bool optional = false) : base(name, locator, optional)
        {
        }

        public virtual void SetText(string text)
        {
            if(Enabled && Displayed)
            {
                Mediator.Execute(() => ElementProvider.SendKeys(text));
            }
            else
            {
                throw new ArgumentException($"Проверьте что элемент {Name} Enabled и Displayed");
            }
        }

        public virtual void Clear(string text)
        {
            if (Enabled && Displayed)
            {
                Mediator.Execute(() => ElementProvider.Clear());
            }
            else
            {
                throw new ArgumentException($"Проверьте что элемент {Name} Enabled и Displayed");
            }
        }
    }
}
