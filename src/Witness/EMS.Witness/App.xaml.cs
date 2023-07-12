using Core.Application.Rpc;
using Core.Events;
using EMS.Witness.Rpc;
using EMS.Witness.Services;
using EMS.Witness.Shared.Toasts;

namespace EMS.Witness;

public partial class App : Application
{
    private readonly IStartlistClient startlistClient;
    private readonly IStartlistService startlistService;
    private readonly IToaster toaster;
    private readonly IRpcService rpcService;
    private readonly IParticipantsClient participantsClient;
    private readonly IParticipantsService participantsService;

    public App(
		IStartlistClient startlistClient,
		IStartlistService startlistService,
		IToaster toaster,
		IEnumerable<IRpcClient> rpcClients,
		IRpcService rpcService,
		IParticipantsClient arrivelistClient,
		IParticipantsService arrivelistService)
	{
		this.InitializeComponent();
        this.MainPage = new MainPage();
        this.startlistClient = startlistClient;
        this.startlistService = startlistService;
        this.toaster = toaster;
        this.rpcService = rpcService;
        this.participantsClient = arrivelistClient;
        this.participantsService = arrivelistService;
        this.HandleRpcErrors(rpcClients);
		this.HandleCoreErrors();
		this.AttachEventHandlers();
    }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = base.CreateWindow(activationState);

		window.Activated += (s, e) => this.rpcService.Handshake();

		return window;
	}

	private void AttachEventHandlers()
	{

        this.participantsClient.Updated += (sender, args) => this.participantsService.Update(args.entry, args.action);
        this.participantsClient.ServerConnectionChanged += async (sender, isConnected) => 
		{
			if (isConnected)
			{
				await this.participantsService.Load();
			}
        };

		this.startlistClient.Updated += (s, a) => this.startlistService.Update(a.entry, a.action);
        this.startlistClient.ServerConnectionChanged += async (sender, isConnected) =>
        {
            if (isConnected)
            {
                await this.startlistService.Load();
            }
        };
    }

	private void HandleRpcErrors(IEnumerable<IRpcClient> rpcClients)
	{
		foreach (var client in rpcClients)
		{
            client.Error += (sender, error) =>
            {
                this.toaster.Add("RPC client error", $"{error.Procedure}: {error.Exception.Message}", UiColor.Danger, 30);
            };
        }
	}

	private void HandleCoreErrors()
	{
		CoreEvents.ErrorEvent += (sender, error) =>
		{
			this.toaster.Add(error.Message, error.StackTrace, UiColor.Danger, 30);
		};
	}
}
