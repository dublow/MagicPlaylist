using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Common.Profilers
{
    public class SmartTimer : IDisposable
    {
        private readonly Stopwatch _stopWatch;
        private readonly Action<Stopwatch, int> _action;
        private int _userId;

        public SmartTimer(Action<Stopwatch, int> action = null)
        {
            _action = action ?? ((s, userId) => Console.WriteLine(s.Elapsed));
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }
        public void Dispose()
        {
            _stopWatch.Stop();
            _action(_stopWatch, _userId);
        }

        public Stopwatch Stopwatch { get { return _stopWatch; } }
        public void SetUserId(int userId)
        {
            _userId = userId;
        }
    }
}
