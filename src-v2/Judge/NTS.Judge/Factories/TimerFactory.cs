using System.Timers;

namespace NTS.Judge.Factories;

public class TimerFactory
{
    public static System.Timers.Timer CreateTimer(
        double tickLength,
        ElapsedEventHandler onTickHandler,
        Func<bool> isActive
    )
    {
        var _timer = new System.Timers.Timer(tickLength);
        _timer.Elapsed += onTickHandler;
        _timer.AutoReset = isActive();
        _timer.Enabled = isActive();
        return _timer;
    }
}
