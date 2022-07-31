using MoodleScraper.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class ElivaStrategy : ScrapeStrategy
    {
        public ElivaStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var nonwebex = DefaultStrategy((text) => text.StartsWith("Lezione"));
            var webex = DefaultStrategy((text) => text.Contains("Registrazione") || text.Contains("Maggio"));

            nonwebex.ForEach((link) => new VideoDownloadStrategy(link.Value, $"Eliva\\{link.Key}.mp4"));
            webex.ForEach((link) => OpenWebexLink(link.Value,link.Key,"Eliva"));

        }
    }
}
