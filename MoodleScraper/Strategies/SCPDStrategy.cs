using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class SCPDStrategy : StrategyBase
    {
        string _url;
        public SCPDStrategy(string url)
        {
            _url = url;
        }
        public override void Execute()
        {
            Driver.Navigate().GoToUrl(_url);

            var instances = Driver.FindElements(By.ClassName("activityinstance"));

            foreach (var instance in instances)
            {
                if (instance.FindElement(By.ClassName("instancename")).Text.StartsWith("L"))
                {
                    new VideoDownloadStrategy(instance.FindElement(By.ClassName("aalink")).GetAttribute("href"), "SCPD\\" + instance.FindElement(By.ClassName("instancename")).Text + ".mp4");
                }
            }
        }
    }
}
