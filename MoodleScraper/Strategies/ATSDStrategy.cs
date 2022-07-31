
using MoodleScraper.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class ATSDStrategy : ScrapeStrategy
    {
        public ATSDStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var nonWebexLink = DefaultStrategy((name) => name.ToLower().Contains("lezioni asincrone"));
            var webexLink = DefaultStrategy((name) => name.ToLower().Contains("registrazione"));

            foreach(var nowe in nonWebexLink) { new VideoDownloadStrategy(nowe.Value, $"atsd/{nowe.Key}.mp4"); };
            foreach(var link in webexLink)
            {
                OpenWebexLink(link.Value, link.Key, "atsd");
            }
        }
    }
}
