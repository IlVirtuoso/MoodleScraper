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

            var links = DefaultStrategy((text) => text.Contains("Video") || text.Contains("Lezione"));
            links.ForEach((link) => new VideoDownloadStrategy(link.Value, $"SimMod\\{link.Key}.mp4"));
        }
    }
}
