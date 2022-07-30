using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Adapters
{
    public abstract class ActionStrategy : StrategyBase
    {
        private Action _exec;
        public ActionStrategy(Action action)
        {
            _exec = action;
        }

        public override void Execute() => _exec();
    }
}
