using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class VideoDownloadStrategy : StrategyBase
    {
        private string _url;
        private string _filePath;
        public VideoDownloadStrategy(string url,string filePath)
        {
            _url = url;
            _filePath = filePath;
        }
        public override void Execute()
        {
            Driver.Navigate().GoToUrl(_url);
            var targets = Driver.FindElements(By.TagName("source"));
            foreach(var target in targets)
            {
                var link = target.GetAttribute("src");
                Downloader.Instance.AddRequest(new DownloadCommit(link, _filePath));
            }

        }
    }
}
