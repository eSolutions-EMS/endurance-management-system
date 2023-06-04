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
    private readonly ToasterService toaster;
    private readonly IRpcService rpcService;
    private readonly IArrivelistClient arrivelistClient;
    private readonly IArrivelistService arrivelistService;

    public App(
		IStartlistClient startlistClient,
		IStartlistService startlistService,
		ToasterService toaster,
		IEnumerable<IRpcClient> rpcClients,
		IRpcService rpcService,
		IArrivelistClient arrivelistClient,
		IArrivelistService arrivelistService)
	{
		this.InitializeComponent();
        this.MainPage = new MainPage();
        this.startlistClient = startlistClient;
        this.startlistService = startlistService;
        this.toaster = toaster;
        this.rpcService = rpcService;
        this.arrivelistClient = arrivelistClient;
        this.arrivelistService = arrivelistService;
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

        this.arrivelistClient.Updated += (sender, args) => this.arrivelistService.Update(args.entry, args.action);
        this.arrivelistClient.ServerConnectionChanged += async (sender, isConnected) => 
		{
			if (isConnected)
			{
				await this.arrivelistService.Load();
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
                var toast = new Toast("RPC client error", $"{error.Procedure}: {error.Exception.Message}", UiColor.Danger, 20);
                this.toaster.Add(toast);
            };
        }
	}

	private void HandleCoreErrors()
	{
		CoreEvents.ErrorEvent += (sender, error) =>
		{
			var toast = new Toast(error.Message, error.StackTrace, UiColor.Danger, 30);
			this.toaster.Add(toast);
		};
	}
}
