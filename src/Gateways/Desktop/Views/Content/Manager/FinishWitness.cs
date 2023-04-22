using EnduranceJudge.Application.Hardware;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.AggregateRoots.Manager.WitnessEvents;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager;

public class FinishWitness : BindableBase
{
    //TODO: provide the ability for users to configure this IP.
    public const string FINISH_DEVICE_IP = "192.168.68.128";

    private readonly Dictionary<string, DateTime> cache = new();
    private readonly VupRfidController controller;
    private string message;

    public FinishWitness()
    {
        this.controller = new VupRfidController(FINISH_DEVICE_IP);
        this.controller.MessageEvent += (_, message) => this.Message = message;
        this.controller.ReadEvent += this.RaiseWitnessEvent;
    }

    public string Message
    {
        get => this.message;
        set => this.SetProperty(ref this.message, this.message);
    }

    public void Connect()
    {
        this.controller.Connect();
    }

    /// <summary>
    /// Executed in a separate Thread due to best practices for background services in WPF
    /// </summary>
    public void Start()
    {
        Task.Run(() => this.controller.StartPolling());
    }

    public void Stop()
    {
        this.controller.StopPolling();
    }

    public void Disconnect()
    {
        Task.Run(() => this.controller.Disconnect());
    }

    public bool IsStarted()
    {
        return this.controller.IsPolling;
    }

    private void RaiseWitnessEvent(object _, IEnumerable<string> tags)
    {
        ThreadPool.QueueUserWorkItem(delegate
        {
            SynchronizationContext.SetSynchronizationContext(new
                DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));

            SynchronizationContext.Current!.Post(pl =>
            {
                foreach (var tagId in tags)
                {
                    var now = DateTime.Now;
                    if (this.cache.ContainsKey(tagId) && now - this.cache[tagId] < TimeSpan.FromMinutes(1))
                    {
                        continue;
                    }
                    this.cache[tagId] = now;

                    var witnessEvent = new WitnessEvent
                    {
                        Type = WitnessEventType.VetIn,
                        TagId = tagId,
                        Time = now,
                    };
                    Witness.Raise(witnessEvent);
                }
            }, null);
        });
    }
}
