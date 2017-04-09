using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Assets.Code
{
    public static class CombatController
    {
        public static TimeSpan GetCombatTimer { get { return _stopwatch.Elapsed; } }

        static Stopwatch _stopwatch = new Stopwatch();

        public static void StartCombatTimer()
        {
            _stopwatch.Start();
        }

        public static void StopCombatTimer()
        {
            _stopwatch.Stop();
        }

        public static void ResetCombatTimer()
        {
            _stopwatch.Reset();
        }

        public static void ResetAndRestartCombatTimer()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public static int GetRemainingTime(TimeSpan timespan)
        {
            var remaining = (timespan - GetCombatTimer).Seconds;
            return remaining > 0 ? remaining : 0;
        }
    }
}
