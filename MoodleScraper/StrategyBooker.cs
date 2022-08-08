using MoodleScraper.Adapters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper
{
    public class StrategyBooker
    {
        private static StrategyBooker? _instance;
        public static StrategyBooker Instance
        {
            get
            {
                _instance = _instance ?? new StrategyBooker();
                return _instance;
            }
        }

        private ConcurrentQueue<IStrategy> _strategies = new ConcurrentQueue<IStrategy>();

        private Semaphore _maxActiveThreads = new Semaphore(16, 16);

        private bool _routineActive = false;
        private Thread? _routine;
        public void RegisterStrategy(IStrategy strategy)
        {
            _maxActiveThreads.WaitOne();
            _strategies.Enqueue(strategy);
        }

        public Thread Start()
        {
            if(_routine== null)
            {
                _routineActive = true;
                _routine = new Thread(() =>
                {
                    while (_routineActive)
                    {
                        if(_strategies.Count > 0)
                        {
                            IStrategy? strategy = null;
                            if(_strategies.TryDequeue(out strategy))
                            {
                                Task.Run(() =>
                                {
                                    if (strategy != null)
                                    {
                                        strategy.Prepare();
                                        strategy.Run();
                                        _maxActiveThreads.Release(1);
                                        strategy.Dispose();
                                    }
                                });
                            }
                        }
                        Thread.Sleep(1);
                    }
                });

                _routine.Start();
            }
            return _routine;
        }


        public void Stop()
        {
            _routineActive = false;
        }



    }
}
