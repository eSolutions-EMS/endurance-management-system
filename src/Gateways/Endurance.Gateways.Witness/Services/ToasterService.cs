using Endurance.Gateways.Witness.Shared.Toasts;
using System.Timers;

namespace Endurance.Gateways.Witness.Services;
public class ToasterService : IDisposable
{
    private readonly List<Toast> toastList = new();
    private readonly System.Timers.Timer timer = new();

    public ToasterService()
    {
        this.timer.Interval = 1000;
        this.timer.AutoReset = true;
        this.timer.Elapsed += this.HandleTimerElapsed;
        this.timer.Start();
    }

    public event EventHandler? ToasterChanged;
    public event EventHandler? ToasterTimerElapsed;
   
    public bool HasToasts => toastList.Count > 0;

    public List<Toast> GetToasts()
    {
        ClearBurntToast();
        return this.toastList.ToList();
    }

    public void AddToast(Toast toast)
    {
        this.toastList.Add(toast);
        if (!this.ClearBurntToast())
        { 
            this.ToasterChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearToast(Toast toast)
    {
        if (this.toastList.Contains(toast))
        {
            this.toastList.Remove(toast);
            if (!this.ClearBurntToast())
            {
                this.ToasterChanged?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public void Dispose()
    {
        if (this.timer is not null)
        {
            this.timer.Elapsed += this.HandleTimerElapsed;
            this.timer.Stop();
        }
    }

    private bool ClearBurntToast()
    {
        var toastsToDelete = this.toastList.Where(item => item.IsBurnt).ToList();
        if (!toastsToDelete.Any())
        {
            return false;
        }
        
        toastsToDelete.ForEach(toast => this.toastList.Remove(toast));
        this.ToasterChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    private void HandleTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        this.ClearBurntToast();
        this.ToasterTimerElapsed?.Invoke(this, EventArgs.Empty);
    }
}
