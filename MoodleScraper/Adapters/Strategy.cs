using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Adapters
{
    public abstract class StrategyBase : IStrategy
    {
        protected IWebDriver Driver { get; private set; }
        private ManualResetEvent _waitEvent = new ManualResetEvent(false);
        private bool _execute = true;

#pragma warning disable CS8618
        public StrategyBase()

        {
            StrategyBooker.Instance.RegisterStrategy(this);
        }
#pragma warning restore CS8618 

        public virtual void Prepare()
        {
            Driver = DriverPool.Instance.TryGetWebDriver();
        }

        public virtual void Run()
        {
            while (_execute)
            {
                _execute = false;
                Execute();
            }
            _waitEvent.Set();
            Driver.Navigate().GoToUrl("about:home");
            DriverPool.Instance.ReleaseDriver(Driver);
        }

        public void Wait()
        {
            _waitEvent.WaitOne();
        }

        protected void RequestRetry()
        {
            _execute = true;
        }

        public abstract void Execute();

        public virtual void Dispose()
        {
            
        }
    }
}
