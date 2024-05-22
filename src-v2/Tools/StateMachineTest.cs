using System.Diagnostics;
using System.Timers;

namespace Tools;

public static class StateMachineTest
{
    public static async Task TestIndependantAwait()
    {
        var delay = TimeSpan.FromSeconds(3);
        var timer = new Stopwatch();
        timer.Start();
        await Task.Delay(delay);
        await Task.Delay(delay);
        timer.Stop();
        Console.WriteLine($"First time to avoid coldstart: {timer.Elapsed}");

        timer.Restart();
        timer.Start();
        await Task.Delay(delay);
        await Task.Delay(delay);
        timer.Stop();
        Console.WriteLine($"Simple: {timer.Elapsed}");

        timer.Reset();
        timer.Start();
        var one = Task.Delay(delay);
        var two = Task.Delay(delay);
        await one;
        await two;
        timer.Stop();
        Console.WriteLine($"Janky: {timer.Elapsed}");
    }
}
