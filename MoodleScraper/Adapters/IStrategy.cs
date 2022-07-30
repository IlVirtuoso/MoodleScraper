using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleScraper.Adapters
{
    public interface IStrategy:IDisposable
    {
        void Prepare();
        void Run();
    }
}
