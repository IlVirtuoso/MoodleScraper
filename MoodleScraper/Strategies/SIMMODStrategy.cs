using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class SIMMODStrategy : ScrapeStrategy
    {
        public SIMMODStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var instances = Driver.FindElements(By.ClassName("activityinstance"));
            foreach(var instance in instances)
            {
                var videoName = instance.FindElement(By.ClassName("instancename"));
                if(videoName.Text.StartsWith("Video") || videoName.Text.StartsWith("Lezione"))
                {
                    var link = instance.FindElement(By.ClassName("aalink")).GetAttribute("href");
                    new VideoDownloadStrategy(link, $"simmod\\{videoName.Text}.mp4");
                }
            }
        }
    }
}
