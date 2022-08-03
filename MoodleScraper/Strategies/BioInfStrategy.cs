using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class BioInfStrategy : ScrapeStrategy
    {
        public BioInfStrategy(string link) : base(link)
        {
        }

        public override void Execute()
        {
            var topLinks = DefaultStrategy((text) => text.Contains("Lezione"));
            foreach(var topLink in topLinks)
            {
                OpenWebexLink(topLink.Value, topLink.Key, "bioinf");
            }
            var pageLink = DefaultStrategy((text) => text.Contains("Registrazioni audio/video")).First().Value;
            foreach (var elem in CatchLinksFromPage(pageLink))
            {
                new WebexDownloadStrategy(elem.Value, $"bioinf\\{elem.Key}.mp4");
            }
        }

        private List<KeyValuePair<string, string>> CatchLinksFromPage(string link)
        {
            Driver.Navigate().GoToUrl(link);
            var links = Driver.FindElements(By.TagName("a")).Where((elem) => elem.GetAttribute("href").Contains("unito.webex.com"));
            return links.Select((elem)=> new KeyValuePair<string, string>(elem.Text,elem.GetAttribute("href"))).ToList();

        }

    }
}
