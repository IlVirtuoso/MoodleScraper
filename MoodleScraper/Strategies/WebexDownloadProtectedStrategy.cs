using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    internal class WebexDownloadProtectedStrategy : WebexDownloadStrategy
    {
        public WebexDownloadProtectedStrategy(string url, string filePath) : base(url, filePath)
        {
        }

        public override void Execute()
        {
            var info = Driver.FindElement(By.ClassName("el-input__inner"));
            info.SendKeys("zzzzz");
            var button = Driver.FindElement(By.ClassName("el-button"));
            button.Click();
            base.Execute();
        }
    }
}
