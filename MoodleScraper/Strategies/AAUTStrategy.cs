using MoodleScraper.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class AAUTStrategy : ScrapeStrategy
    {
        public AAUTStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var nonwebex = DefaultStrategy((text) => text.Contains("Lezione"));
            var webex = DefaultStrategy((text) => text.Contains("Lecture"));

            nonwebex.ForEach((link) => new VideoDownloadStrategy(link.Value, $"aaut\\{link.Key}.mp4"));
            webex.ForEach((link) => OpenWebexLink(link.Value, link.Key, "aaut"));

        }
    }
}
