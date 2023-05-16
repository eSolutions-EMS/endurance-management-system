using Prism.Mvvm;
using System;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Hardware.Tags;

public class TagViewModel : BindableBase
{
    private string id;
    private int detectedCount;

    public string Id
    {
        get => this.id;
        set => this.SetProperty(ref this.id, value);
    }
    public int DetectedCount
    {
        get => this.detectedCount;
        set => this.SetProperty(ref this.detectedCount, value);
    }
    private string name;
    public string Name
    {
        get => this.name;
        set => this.SetProperty(ref this.name, value);
    }

    private SolidColorBrush color = new SolidColorBrush(Colors.SpringGreen);
    public SolidColorBrush Color
    {
        get => this.color;
        set => this.SetProperty(ref this.color, value);
    }

    public void Detect()
    {
        this.DetectedCount++;
        this.Color = new SolidColorBrush(Colors.SpringGreen);
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            ThreadPool.QueueUserWorkItem(delegate
            {
                SynchronizationContext.SetSynchronizationContext(new
                    DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));

                SynchronizationContext.Current!.Post(_ =>
                {
                    this.Color = new SolidColorBrush(Colors.White);
                }, null);
            });
        });
    }
}
