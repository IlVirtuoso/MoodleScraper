using MoodleScraper.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class MTNStrategy : ScrapeStrategy
    {
        public MTNStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var links = DefaultStrategy((text) => text.Contains("registrazione"));
            links.ForEach((link) => OpenWebexLink(link.Value, link.Key, "metodinumerici"));
        }
    }
}
