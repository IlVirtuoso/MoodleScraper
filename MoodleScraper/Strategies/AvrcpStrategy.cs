using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class AvrcpStrategy : StrategyBase
    {
        private string _url;
        public AvrcpStrategy(string url)
        {
            _url = url;
        }
        public override void Execute()
        {
            
            Driver.Navigate().GoToUrl(_url);
            var contents = Driver.FindElements(By.ClassName("content"));
            
            foreach(var content in contents)
            {
                var section = content.FindElements(By.ClassName("sectionname")).FirstOrDefault();
                if(section != null)
                {
                    DownloadWeek(section.Text, content);
                }

            }
            return;

        }

        private void DownloadWeek(string weekName, IWebElement section)
        {
            var contents = section.FindElements(By.ClassName("contentwithoutlink"));
            string lectureName = "";
            string lectureLink = "";
            foreach(var content in contents)
            {
                IWebElement? strong = content.FindElements(By.TagName("strong")).FirstOrDefault();
                IWebElement? href = content.FindElements(By.TagName("a")).FirstOrDefault();

                if (strong != null) lectureName = strong.Text;
                if(href != null && href.GetAttribute("href") != null)
                {
                    lectureLink = href.GetAttribute("href");
                    new WebexDownloadStrategy(lectureLink, $"avrcp\\{lectureName}.mp4");
                    Console.WriteLine($"{lectureName} : {lectureLink}");
                }
            }

        }

    }
}
