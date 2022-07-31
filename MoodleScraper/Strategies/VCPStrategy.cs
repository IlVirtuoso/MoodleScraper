using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class VCPStrategy : ScrapeStrategy
    {
        public VCPStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var instances = Driver.FindElements(By.ClassName("activityinstance"));
            List<KeyValuePair<string, string>> links = new List<KeyValuePair<string, string>>();
            foreach (var instance in instances)
            {
                var instanceName = instance.FindElement(By.ClassName("instancename"));
                if (instanceName.Text.ToLower().Contains("lezione"))
                {

                    var link = instance.FindElement(By.TagName("a")).GetAttribute("href");
                    links.Add(new KeyValuePair<string, string>(instanceName.Text, link));
                }
            }

            foreach(var link in links)
            {
                OpenLink(link.Value, link.Key);
            }
        }

        private void OpenLink(string link, string videoName)
        {
            Driver.Navigate().GoToUrl(link);

            var urlworkaround = Driver.FindElements(By.ClassName("urlworkaround")).FirstOrDefault();
            if (urlworkaround != null)
            {
                var videoLink = urlworkaround.FindElement(By.TagName("a")).GetAttribute("href");
                new WebexDownloadStrategy(videoLink, $"verificaprogrammiconcorrenti\\{videoName}.mp4");
            }
        }
    }
}
