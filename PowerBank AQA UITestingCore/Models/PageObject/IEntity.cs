using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITestingCore.Models.PageObject
{
    public interface IEntity
    {
        Node Root { get; set; }

        string Name { get; set; }

        How How { get; set; }

        string Locator { get; set; }

        By By { get;}

        bool Optional { get; set; }
    }
}
