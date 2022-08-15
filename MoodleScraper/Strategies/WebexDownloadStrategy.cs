using MoodleScraper.Adapters;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V104;
using OpenQA.Selenium.DevTools.V104.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using V104 = OpenQA.Selenium.DevTools.V104;
namespace MoodleScraper.Strategies
{
    public class WebexDownloadStrategy : ScrapeStrategy
    {
        private string? _result;
        private string _path;

        public string Result
        {
            get
            {
                if (_result == null) Wait();
                return _result ?? throw new ApplicationException("Link not found in page");
            }
        }
        public WebexDownloadStrategy(string url, string filePath) : base(url)
        {
            _path = filePath;
        }

        public override void Prepare()
        {
            base.Prepare();
            (Driver as IDevTools).GetDevToolsSession().Domains.Network.EnableNetwork();
            //Driver.Manage().Network.StartMonitoring().Wait();
        }

        public override void Run()
        {
            base.Run();
            Driver.Manage().Network.StopMonitoring().Wait();
            //(Driver as IDevTools).GetDevToolsSession().Domains.Network.DisableNetwork();
        }

        public override void Execute()
        {
            ManualResetEvent linkCaptured = new ManualResetEvent(false);
            ManualResetEvent pageLoaded = new ManualResetEvent(false);
            void OnResponse(object sender, V104.Network.RequestWillBeSentEventArgs e)
            {

                if (e.Request.Url.Contains("m3u8"))
                {
                    _result = e.Request.Url;
                    linkCaptured.Set();
                }

            };


            (Driver as IDevTools).GetDevToolsSession().GetVersionSpecificDomains<V104.DevToolsSessionDomains>().Network.RequestWillBeSent += OnResponse;
            while (Driver.FindElements(By.ClassName("vjs-big-play-button")).Count == 0) { Thread.Sleep(1); }
            var playButton = Driver.FindElement(By.ClassName("vjs-big-play-button"));
            playButton.Click();
            linkCaptured.WaitOne();
            (Driver as IDevTools).GetDevToolsSession().GetVersionSpecificDomains<V104.DevToolsSessionDomains>().Network.RequestWillBeSent -= OnResponse;
            if (_result != null)
                Downloader.Instance.AddRequest(new DownloadCommit(_result, _path));
        }


    }
}
