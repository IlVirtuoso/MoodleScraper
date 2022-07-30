using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;

namespace MoodleScraper.Adapters
{
    public abstract class StrategyBase : IStrategy
    {
        protected IWebDriver Driver { get; private set; }
        protected ProxyServer Proxy { get; private set; }
        private ManualResetEvent _waitEvent = new ManualResetEvent(false);

#pragma warning disable CS8618
        public StrategyBase()

        {
            StrategyBooker.Instance.RegisterStrategy(this);
        }
#pragma warning restore CS8618 

        public virtual void Prepare()
        {
            Driver = DriverPool.Instance.TryGetWebDriver();
            Proxy = DriverPool.Instance.GetProxy(Driver);
        }

        public void Run()
        {
            Proxy.Start();
            Execute();
            _waitEvent.Set();
            Driver.Navigate().GoToUrl("about:home");
            Proxy.Stop();
        }

        public void Wait()
        {
            _waitEvent.WaitOne();
        }

        public abstract void Execute();

        public void Dispose()
        {
            if(Driver != null)
                DriverPool.Instance.ReleaseDriver(Driver);
        }
    }
}
