using System;
using System.Threading;

namespace Serilog.Generator
{
    sealed class ActiveAgent : IDisposable
    {
        readonly int _maxMsRest;
        readonly Action[] _activities;
        readonly Random _random = new Random();
        readonly Timer _timer;

        public ActiveAgent(int maxMsRest, params Action[] activities)
        {
            _maxMsRest = maxMsRest;
            _activities = activities;
            _timer = new Timer(s => Act());
        }

        public void Start()
        {
            Schedule();
        }

        void Schedule()
        {
            var rest = (int) (_random.NextDouble() * _maxMsRest);
            try
            {
                _timer.Change(rest, Timeout.Infinite);
            }
            catch (ObjectDisposedException oex)
            {
                Log.Error(oex, "Timer cannot be reset because it is disposed");
            }
        }

        void Act()
        {
            try
            {
                var ai = _random.Next() % _activities.Length;
                if (ai == _activities.Length) ai -= 1;
                _activities[ai]();
                Schedule();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error performing activity");
            }
        }

        public void Dispose()
        {
            var wh = new AutoResetEvent(false);
            if (_timer.Dispose(wh))
                wh.WaitOne();
        }
    }
}