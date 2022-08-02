using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class BDMStrategy : ScrapeStrategy
    {
        public BDMStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var links = Driver.FindElements(By.TagName("a")).Where((elem) => elem.Text.Contains("Sala riunioni personale di Maria Luisa")).ToList();
            int i = 0;
            links.ForEach((link) => new WebexDownloadStrategy(link.GetAttribute("href"), $"BDM\\registrazione{i++}.mp4"));
        }
    }
}
