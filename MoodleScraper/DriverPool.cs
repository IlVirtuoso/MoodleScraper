using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;

namespace MoodleScraper
{
    public class DriverPool
    {
        public static DriverPool? _instance;
        public static DriverPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DriverPool();
                    _instance.SetMaxPool(1);
                }
                return _instance;
            }
        }

        public static bool HeadlessDrivers { get; set; } = false;

        private DriverManager _manager = new DriverManager();

        private List<IWebDriver> _pools = new List<IWebDriver>();

        private List<bool> _states = new List<bool>();

        private event EventHandler? OnPoolRelease;

        private Mutex _access = new Mutex();

        private List<ProxyServer> _proxies = new List<ProxyServer>();

        public void SetMaxPool(int pools)
        {
            _pools.ForEach((d) => d.Dispose());
            _pools.Clear();
            _states.Clear();
            _proxies.ForEach((p) => p.Dispose());
            _proxies.Clear();
            for(int i = 0; i < pools; i++)
            {
                _proxies.Add(_manager.CreateProxy(4443 + i));
                _pools.Add(_manager.CreateDriver(proxyPort:4443+i,headless:HeadlessDrivers));
                _states.Add(true);
            }
        }

        public ProxyServer GetProxy(IWebDriver driver)
        {
            return _proxies[_pools.IndexOf(driver)];
        }

        private void WaitFreeDriver()
        {
            using (AutoResetEvent wait = new AutoResetEvent(false))
            {
                void set(object? sender, EventArgs? e) => wait.Set();
                OnPoolRelease += set;
                wait.WaitOne();
                OnPoolRelease -= set;
            }
        }

        public void ReleaseDriver(IWebDriver driver)
        {
            _access.WaitOne();
            int index = _pools.IndexOf(driver);
            _states[index] = true;
            OnPoolRelease?.Invoke(this, new EventArgs());
            _access.ReleaseMutex();
        }

        public IWebDriver TryGetWebDriver()
        {
            _access.WaitOne();

            while(!_states.Any((s) => s))
            {
                _access.ReleaseMutex();
                WaitFreeDriver();
                _access.WaitOne();
            }

            int index = 0;
            for(index = 0; index < _states.Count; index++)
            {
                if (_states[index]) break;  
            }
            var driver = _pools[index];
            _states[index] = false;
            _access.ReleaseMutex();
            return driver;
        }










    }
}
