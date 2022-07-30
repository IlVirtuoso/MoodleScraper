using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class LoginStrategy : StrategyBase
    {
        public override void Execute()
        {
            
            Driver.Navigate().GoToUrl("https://informatica.i-learn.unito.it/login/index.php");
            var username = Driver.FindElement(By.Id("username"));
            var password = Driver.FindElement(By.Id("password"));
            var form = Driver.FindElement(By.Id("login"));
            username.SendKeys("matteo.ielacqua");
            password.SendKeys("?M1a2t3t4e5o6");
            form.Submit();
        }
    }
}
