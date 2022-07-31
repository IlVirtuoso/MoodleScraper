using MoodleScraper.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class ModelGraphicStrategy : ScrapeStrategy
    {
        public ModelGraphicStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var nonwebexlink = DefaultStrategy((text) => text.Contains("Videomateriale") || text.Contains("Lezione"));
            nonwebexlink.ForEach((link) => new VideoDownloadStrategy(link.Value, $"modelgrap\\{link.Key}.mp4"));

            //missing lab part
        }
    }
}
