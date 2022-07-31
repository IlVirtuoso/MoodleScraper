using MoodleScraper.Adapters;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Strategies
{
    public class WebexDownloadStrategy : StrategyBase
    {
        private string? _result;
        private string _path;
        private string _url;
        public string Result
        {
            get
            {
                if (_result == null) Wait();
                return _result ?? throw new ApplicationException("Link not found in page");
            }
        }
        public WebexDownloadStrategy(string url, string filePath) : base()
        {
            _url = url;
            _path = filePath;
        }

        public override void Prepare()
        {
            base.Prepare();
            Driver.Manage().Network.StartMonitoring().Wait();
        }

        public override void Run()
        {
            base.Run();
            Driver.Manage().Network.StopMonitoring().Wait();
        }

        public override void Execute()
        {
            ManualResetEvent linkCaptured = new ManualResetEvent(false);
            ManualResetEvent pageLoaded = new ManualResetEvent(false);
            void OnResponse(object sender, NetworkRequestSentEventArgs e)
            {

                if (e.RequestUrl.Contains("m3u8"))
                {
                    _result = e.RequestUrl;
                    linkCaptured.Set();
                }

            };

            Driver.Manage().Network.NetworkRequestSent += OnResponse;
            Driver.Navigate().GoToUrl(_url);
            while (Driver.FindElements(By.ClassName("vjs-big-play-button")).Count == 0) { Thread.Sleep(1); }
            var playButton = Driver.FindElement(By.ClassName("vjs-big-play-button"));
            playButton.Click();
            linkCaptured.WaitOne();
            Driver.Manage().Network.NetworkRequestSent -= OnResponse;
            if (_result != null)
                Downloader.Instance.AddRequest(new DownloadCommit(_result, _path));
        }


    }
}
