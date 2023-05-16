using EMS.Judge.Core.Services;
using EMS.Judge.Application.Hardware;
using EMS.Judge.Application.Services;
using EMS.Core.Events;
using EMS.Core.Domain.AggregateRoots.Manager;
using EMS.Core.Domain.AggregateRoots.Manager.WitnessEvents;
using EMS.Core.Domain.State;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EMS.Judge.Views.Content.Manager;

public class RfidWitness : IRfidWitness
{
    private readonly ISettings settings;
    private readonly IPopupService popupService;
    private readonly IState state;

    //TODO: provide the ability for users to configure this IP.
    public const string FINISH_DEVICE_IP = "192.168.68.128";
    private readonly Dictionary<string, DateTime> cache = new();
    private readonly VupRfidController controller;
    private WitnessEventType type = WitnessEventType.Invalid;

    public RfidWitness(ISettings settings, IPopupService popupService, IState state)
    {
        this.settings = settings;
        this.popupService = popupService;
        this.state = state;
        if (this.settings.IsSandboxMode)
        {
            return;
        }
        this.controller = new VupRfidController(FINISH_DEVICE_IP);
        this.controller.MessageEvent += (_, message) => this.RenderMessage(message);
        this.controller.ReadEvent += this.RaiseWitnessEvent;
        CoreEvents.StateLoadedEvent += this.HandleStateLoaded;
    }

    public void Configure(WitnessEventType type)
    {
        if (this.type != WitnessEventType.Invalid)
        {
            return;
        }
        this.type = type;
    }

    public void Connect()
    {
        if (this.settings.IsSandboxMode)
        {
            return;
        }
        this.controller.Connect();
    }

    public void Reconnect()
    {
        if (this.settings.IsSandboxMode)
        {
            return;
        }
        this.Stop();
        this.controller.Disconnect();
        this.controller.Connect();
        Task.Run(() => this.controller.StartPolling());
    }

    /// <summary>
    /// Executed in a separate Thread due to best practices for background services in WPF
    /// </summary>
    public void Start()
    {
        if (this.settings.IsSandboxMode)
        {
            return;
        }
        if (!this.controller.IsConnected)
        {
            this.controller.Connect();
        }
        Task.Run(() => this.controller.StartPolling());
    }

    public void Stop()
    {
        if (this.settings.IsSandboxMode)
        {
            return;
        }
        this.controller.StopPolling();
    }

    public void Disconnect()
    {
        if (this.settings.IsSandboxMode)
        {
            return;
        }
        if (this.IsNotListening())
        {
            this.Stop();
        }
        Task.Run(() => this.controller.Disconnect());
    }

    public bool IsNotListening()
    {
        return !this.controller.IsPolling;
    }

    private void HandleStateLoaded(object _, EventArgs __)
    {
        if (this.state.Event.HasStarted && this.IsNotListening())
        {
            this.Start();
        }
    }

    private void RenderMessage(string message)
    {
        App.Current.Dispatcher.Invoke(delegate
        {
            this.popupService.RenderError(message);
        });
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
                        Type = this.type,
                        TagId = tagId,
                        Time = now,
                    };
                    Witness.Raise(witnessEvent);
                }
            }, null);
        });
    }
}

public interface IRfidWitness
{
    void Configure(WitnessEventType type);
    void Connect();
    void Reconnect();
    void Start();
    void Stop();
    void Disconnect();
    bool IsNotListening();
}
