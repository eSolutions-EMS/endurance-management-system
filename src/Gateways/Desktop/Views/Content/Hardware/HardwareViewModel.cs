using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Hardware.Tags;
using Microsoft.AspNetCore.Identity;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Vup.reader;
using System.Linq;

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
    }

    public ObservableCollection<TagViewModel> Tags { get; } = new ObservableCollection<TagViewModel>();
    
    public DelegateCommand Connect { get; } 
    public DelegateCommand Start { get; } 
    public DelegateCommand Stop { get; }
    private string _message = "";
    public string Message
    {
        get => this._message;
        set => this.SetProperty(ref this._message, value);
    }

    private bool _isListing = false;
    private VupReader _reader;
    
    public void ConnectAction()
    {
        Task.Run(() =>
        {
            this._reader = new NetVupReader("192.168.68.120", 1969, transport_protocol.tcp);

            var ret = this._reader.Connect();
            if (ret.Success)
            {
                Message = "Connection Successful";
            }
            else
            {
                Message = ret.Message ?? "Unknown failure";
            }

            return false;
        });
    }

    public void StartAction() 
        => Task.Run(() =>
        {
            this._isListing = true;

            var count = this._reader.GetAntCount();
            if (!count.Success)
            {
                // Message = "Get Antenna Number Fail";
                this._isListing = false;
                return;
            }

            while (this._isListing)
            {
                for (int i = 1; i <= count.Result; i++)
                {
                    this._reader.SetWorkAnt(i);
                    var power = this._reader.GetAntPower(i);
                    var ret = this._reader.List6C(memory_bank.memory_bank_epc, 0, 12, Convert.FromHexString("00000000"));

                    if (!ret.Success)
                    {
                        // Message = "List Tag Fail";
                    }
                    else
                    {
                        ThreadPool.QueueUserWorkItem((delegate(object state)
                        {
                            SynchronizationContext.SetSynchronizationContext(new
                                DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));

                            SynchronizationContext.Current.Post(pl =>
                            {
                                
                                var result = ret.Result;
                                foreach (var t in result.Where(x =>
                                    x.Id[0] != 250 || x.Id[1] != 209 || x.Id[2] != 0 || x.Id[3] != 0 || x.Id[4] != 0 ||
                                    x.Id[5] != 0   || x.Id[6] != 0   || x.Id[7] != 0 || x.Id[8] != 0 || x.Id[9] != 0))
                                {
                                    SystemSounds.Beep.Play();
                                    var hex = Convert.ToHexString(t.Id);
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

                    Thread.Sleep(1);
                }
            }
        });

    public void StopAction()
    {
        this._isListing = false;
    }

}
