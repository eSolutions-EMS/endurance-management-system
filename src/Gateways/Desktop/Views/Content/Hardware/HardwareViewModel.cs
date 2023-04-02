using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Hardware.Tags;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Threading;
using Vup.reader;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Hardware;

public class HardwareViewModel : ViewModelBase
{
    private readonly IExecutor executor;

    public HardwareViewModel(IExecutor executor)
    {
        this.executor = this.executor;
        this.Connect = new DelegateCommand(this.ConnectAction);
        this.Start = new DelegateCommand(this.StartAction);
        this.Stop = new DelegateCommand(this.StopAction);
        this.SetPower = new DelegateCommand(this.SetPowerAction);
        this.Reset = new DelegateCommand(this.ResetAction);
    }

    public ObservableCollection<TagViewModel> Tags { get; } = new();
    public DelegateCommand Connect { get; }
    public DelegateCommand Start { get; }
    public DelegateCommand Stop { get; }
    public DelegateCommand SetPower { get; }
    public DelegateCommand Reset { get; }

    private string _message = "";
    private int _power = 27;
    public string Message
    {
        get => this._message;
        set => this.SetProperty(ref this._message, value);
    }
    public string Power
    {
        get => this._power.ToString();
        set => this.SetProperty(ref this._power, int.Parse(value));
    }
    public bool IsListing
    {
        get => this._isListing;
        set
        {
            this.SetProperty(ref this._isListing, value);
            this.RaisePropertyChanged(nameof (this.IsNotListing));
        }
    }
    public bool IsNotListing
    {
        get => !this._isListing;
    }

    private bool _isListing;
    private VupReader _reader;

    public void ConnectAction()
    {
        this._reader = new NetVupReader("192.168.68.128", 1969, transport_protocol.tcp);
        var ret = this._reader.Connect();
        this.Message = ret.Success
            ? "Connection Successful"
            : ret.Message ?? "Unknown failure";
    }

    public void StartAction() => Task.Run(async () =>
    {
        var count = this._reader.GetAntCount();
        if (!count.Success)
        {
            this.Message = "Get Antenna Number Fail";
            return;
        }
        this.IsListing = true;

        while (this.IsListing)
        {
            for (int i = 1; i <= count.Result; i++)
            {
                this._reader.SetWorkAnt(i);
                var ret = this._reader.List6C(
                    memory_bank.memory_bank_epc,
                    0,
                    12,
                    Convert.FromHexString("00000000"));

                if (!ret.Success)
                {
                    // Message = "List Tag Fail";
                }
                else
                {
                    ThreadPool.QueueUserWorkItem((delegate(object? state)
                    {
                        SynchronizationContext.SetSynchronizationContext(new
                            DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));

                        SynchronizationContext.Current.Post(pl =>
                        {
                            var result = ret.Result;
                            // result = result.Where(x =>
                            //     x.Id[0] != 250 || x.Id[1] != 209 || x.Id[2] != 0 || x.Id[3] != 0 || x.Id[4] != 0 ||
                            //     x.Id[5] != 0 || x.Id[6] != 0 || x.Id[7] != 0 || x.Id[8] != 0 || x.Id[9] != 0)
                            //     .ToList();
                            foreach (var t in result)
                            {
                                var hex = Convert.ToHexString(t.Id);
                                SystemSounds.Beep.Play();
                                var existingTag = this.Tags.FirstOrDefault(x => x.Id == hex);
                                if (existingTag != null)
                                {
                                    existingTag.DetectedCount++;
                                }
                                else
                                {
                                    var tag = new TagViewModel { DetectedCount = 1, Id = hex };
                                    this.Tags.Add(tag);
                                }
                            }
                        }, null);
                    }));

                }
                await Task.Delay(TimeSpan.FromMilliseconds(1));
            }
        }
    });

    public void StopAction()
    {
        this.IsListing = false;
    }

    public void SetPowerAction()
    {
        if (this._reader == null)
        {
            this.Message = "Connect to reader first!";
            return;
        }
        var antennas = this._reader.GetAntCount().Result;
        for (var i = 1; i <= antennas; i++)
        {
            var result = this._reader.SetAntPower(i, this._power);
            this.Message = result.Success
                ? $"Antenna power set to {this._power}"
                : $"Cannot set antenna power to {this._power}";
        }
    }

    public void ResetAction()
    {
        foreach (var tag in this.Tags)
        {
            tag.DetectedCount = 0;
        }
    }
}
