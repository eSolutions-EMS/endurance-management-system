using Core.Application.Rpc;
using Core.Domain.AggregateRoots.Manager.Aggregates.Participants;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Enums;
using Core.Events;
using EMS.Witness.Rpc;
using EMS.Witness.Services;
using EMS.Witness.Shared.Toasts;

namespace EMS.Witness;

public partial class App : Application, IDisposable
{
    private readonly IPersistenceService persistence;
	private readonly IEnumerable<IRpcClient> _rpcClients;
    private readonly IStartlistClient startlistClient;
    private readonly IStartlistService startlistService;
    private readonly IToaster toaster;
    private readonly IRpcInitalizer rpcService;
    private readonly IParticipantsClient participantsClient;
    private readonly IParticipantsService participantsService;
	private bool _areRpcConnectionsStarting;
	private object lockObject = new();

    public App(
		IPersistenceService persistence,
		IStartlistClient startlistClient,
		IStartlistService startlistService,
		IToaster toaster,
		IEnumerable<IRpcClient> rpcClients,
		IRpcInitalizer rpcService,
		IParticipantsClient arrivelistClient,
		IParticipantsService arrivelistService)
	{
		this.InitializeComponent();
        this.MainPage = new MainPage();
		_rpcClients = rpcClients;
        this.persistence = persistence;
        this.startlistClient = startlistClient;
        this.startlistService = startlistService;
        this.toaster = toaster;
        this.rpcService = rpcService;
        this.participantsClient = arrivelistClient;
        this.participantsService = arrivelistService;
    }

	protected override Window CreateWindow(IActivationState? activationState) 
	{
		var window = base.CreateWindow(activationState);

		this.AttachEventHandlers();

		window.Created += StartRpcConnections;
		window.Activated += StartRpcConnections;
		window.Resumed += StartRpcConnections;
		window.Deactivated += StoreState;
		window.Destroying += DetachEventHandlers;

		return window;
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	private async void StartRpcConnections(object? sernder, EventArgs args)
	{
		lock (lockObject)
		{
            if (_areRpcConnectionsStarting)
            {
                return;
            }
            _areRpcConnectionsStarting = true;
        }
        await this.rpcService.StartConnections();
    }

    private async void StoreState(object? sender, EventArgs args)
	{
		await this.persistence.Store();
	}

	private void AttachEventHandlers()
	{
		CoreEvents.ErrorEvent += HandleCoreErrors;
		foreach (var client in _rpcClients)
		{
			client.Error += HandleRpcErrors;
		}
		this.participantsClient.Updated += HandleParticipantsUpdate;
		this.participantsClient.ServerConnectionChanged += HandleParticipantsConnectionChanged;
		this.startlistClient.Updated += HandleStartlistUpdate;
		this.startlistClient.ServerConnectionChanged += HandleStartlistConnectionChanged;
    }

	private void DetachEventHandlers(object? sender, EventArgs args)
	{
		CoreEvents.ErrorEvent -= HandleCoreErrors;
		foreach (var client in _rpcClients)
		{
			client.Error -= HandleRpcErrors;
		}
		this.participantsClient.Updated -= HandleParticipantsUpdate;
		this.participantsClient.ServerConnectionChanged -= HandleParticipantsConnectionChanged;
		this.startlistClient.Updated -= HandleStartlistUpdate;
		this.startlistClient.ServerConnectionChanged -= HandleStartlistConnectionChanged;
	}

	private void HandleParticipantsUpdate(object? sender, (ParticipantEntry Participant, CollectionAction Action) args)
	{
		this.participantsService.Update(args.Participant, args.Action);
	}

	private void HandleStartlistUpdate(object? sender, (StartlistEntry StartlistEntry, CollectionAction Action) args)
	{
		this.startlistService.Update(args.StartlistEntry, args.Action);
	}

	private async void HandleParticipantsConnectionChanged(object? sender, RpcConnectionStatus status)
	{
		if (status == RpcConnectionStatus.Connected)
		{
			await this.participantsService.Load();
		}
	}

	private async void HandleStartlistConnectionChanged(object? sender, RpcConnectionStatus status)
	{
		if (status == RpcConnectionStatus.Connected)
		{
			await this.startlistService.Load();
		}
	}

	private void HandleRpcErrors(object? sender, RpcError error)
	{
        this.toaster.Add("RPC client error", $"{error.Procedure}: {error.Exception.Message}", UiColor.Danger, 30);
	}

	private void HandleCoreErrors(object? sender, Exception error)
	{
		this.toaster.Add(error.Message, error.StackTrace, UiColor.Danger, 30);
	}
}
