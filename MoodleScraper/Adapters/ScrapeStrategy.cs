using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Adapters
{
    public abstract class ScrapeStrategy: StrategyBase
    {
        protected string LinkRequest { get; private set; }
        public ScrapeStrategy(string link):base()
        {
            LinkRequest = link;
        }

        public override void Run()
        {
            Driver.Navigate().GoToUrl(LinkRequest);
            base.Run();
        }
    }
}
