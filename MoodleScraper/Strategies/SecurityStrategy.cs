using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class SecurityStrategy : ScrapeStrategy
    {
        public SecurityStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var instances = Driver.FindElements(By.ClassName("activityinstance"));
            List<KeyValuePair<string, string>> videos = new List<KeyValuePair<string, string>>();
            foreach (var instance in instances)
            {
                var videoName = instance.FindElement(By.ClassName("instancename"));
                if (videoName.Text.StartsWith("registrazione"))
                {
                    var link = instance.FindElement(By.ClassName("aalink")).GetAttribute("href");
                    videos.Add(new KeyValuePair<string, string>(videoName.Text, link));
                }
            }

            foreach (var video in videos)
            {
                OpenLink(video.Value, video.Key);
            }
        }

        private void OpenLink(string link, string videoName)
        {
            Driver.Navigate().GoToUrl(link);

            var urlworkaround = Driver.FindElements(By.ClassName("urlworkaround")).FirstOrDefault();
            if (urlworkaround != null)
            {
                var videoLink = urlworkaround.FindElement(By.TagName("a")).GetAttribute("href");
                new WebexDownloadStrategy(videoLink, $"sicurezza2\\{videoName}.mp4");
            }



        }
    }
}
