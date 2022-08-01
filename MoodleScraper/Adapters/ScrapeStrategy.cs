using MoodleScraper.Strategies;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Adapters
{
    public abstract class ScrapeStrategy: StrategyBase
    {
        protected string LinkRequest { get; private set; }
        public ScrapeStrategy(string link):base()
        {
            LinkRequest = link;
        }

        public override void Run()
        {
            Driver.Navigate().GoToUrl(LinkRequest);
            base.Run();
        }

        protected void OpenWebexLink(string link, string videoName,string folderName)
        {
            Driver.Navigate().GoToUrl(link);
            var links = Driver.FindElements(By.TagName("a"));
            foreach(var localLink in links)
            {
                if (localLink.GetAttribute("href").Contains("unito.webex"))
                {
                    new WebexDownloadStrategy(localLink.GetAttribute("href"),$"{folderName}\\{videoName}.mp4");
                    return;
                }
            }
        }

        protected List<KeyValuePair<string,string>> DefaultStrategy(Func<string,bool> PredicateText)
        {
            var instances = Driver.FindElements(By.ClassName("activityinstance"));
            List<KeyValuePair<string, string>> videos = new List<KeyValuePair<string, string>>();
            foreach (var instance in instances)
            {
                var videoName = instance.FindElement(By.ClassName("instancename"));
                if (PredicateText(videoName.Text))
                {
                    var link = instance.FindElement(By.ClassName("aalink")).GetAttribute("href");
                    videos.Add(new KeyValuePair<string, string>(videoName.Text, link));
                }
            }
            return videos;
        }
    }
}
