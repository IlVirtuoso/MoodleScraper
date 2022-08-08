using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MoodleScraper
{
    public class DriverPool
    {
        public static DriverPool? _instance;
        private static object _lock = new object();
        public static DriverPool Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DriverPool();
                        _instance.SetMaxPool(1);
                    }
                    return _instance;
                }
            }
        }

        public static bool HeadlessDrivers { get; set; } = false;

        private DriverManager _manager = new DriverManager();

        private List<IWebDriver> _pools = new List<IWebDriver>();

        private List<bool> _states = new List<bool>();

        private event EventHandler? OnPoolRelease;

        private Semaphore _access = new Semaphore(1, 1);


        public void SetMaxPool(int pools)
        {
            _pools.ForEach((d) => d.Dispose());
            _pools.Clear();
            _states.Clear();
            for(int i = 0; i < pools; i++)
            {
                _pools.Add(_manager.CreateDriver(HeadlessDrivers));
                _states.Add(true);
            }
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
            _access.Release();
        }

        public IWebDriver TryGetWebDriver()
        {
            _access.WaitOne();

            while(!_states.Any((s) => s))
            {
                _access.Release();
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
            _access.Release();
            return driver;
        }










    }
}
