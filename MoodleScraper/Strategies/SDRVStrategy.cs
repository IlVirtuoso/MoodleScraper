using MoodleScraper.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class SDRVStrategy : ScrapeStrategy
    {
        public SDRVStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var nonwebex = DefaultStrategy((text) => text.Contains("Lezioni"));
            var webex = DefaultStrategy((text) => text.Contains("Registrazione"));

            nonwebex.ForEach((link) => new VideoDownloadStrategy(link.Value, $"realtavirt\\{link.Key}.mp4"));
            webex.ForEach((link) => OpenWebexLink(link.Value, link.Key, "realtavirt"));
        }
    }
}
