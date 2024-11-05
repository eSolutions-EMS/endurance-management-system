using System.Timers;

namespace Not.TimerUtility;
public class TimerUtility
{
    public TimerUtility(double tickLength, ElapsedEventHandler onTickHandler, Func<bool> isActive)
    {
        _timer = new System.Timers.Timer(tickLength);
        _timer.Elapsed += onTickHandler;
        _timer.AutoReset = isActive();
        _timer.Enabled = isActive();
    }

    public System.Timers.Timer _timer { get; set; }

    public void StopTimer()
    {
        _timer.Stop(); 
    }
}
