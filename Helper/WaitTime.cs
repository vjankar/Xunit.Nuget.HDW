using System;
using System.Diagnostics;
using System.Threading;

namespace selenium.xunit.framework.package.Helper
{
    public static class WaitTime
    {
        public static void WaitForResult(Func<bool> conditionToWaitFor, int timeout = 300)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (!conditionToWaitFor.Invoke())
        {
            if (stopwatch.Elapsed.Seconds > timeout)
            {
                stopwatch.Stop();
                throw new TimeoutException("...Result timed out");
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"...Waited for {stopwatch.Elapsed.Seconds}s");
    }

    public static void Wait(int timeInSeconds)
    {
        Thread.Sleep((int)timeInSeconds * 1000);
    }
}
}
