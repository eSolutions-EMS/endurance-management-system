using Core.Application.Rpc;
using Core.Events;
using EMS.Witness.Services;
using EMS.Witness.Shared.Toasts;

namespace EMS.Witness;

public partial class App : Application
{
	private readonly IApiService apiService;
    private readonly ToasterService toaster;
    private readonly IEnumerable<IRpcClient> rpcClients;

    public App(IApiService apiService, ToasterService toaster, IEnumerable<IRpcClient> rpcClients)
	{
		this.InitializeComponent();
        this.MainPage = new MainPage();

        this.apiService = apiService;
        this.toaster = toaster;
        this.rpcClients = rpcClients;

		this.HandleRpcErrors();
		this.HandleCoreErrors();
    }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = base.CreateWindow(activationState);

		window.Activated += async (s, e) => await this.apiService.FetchInitialState();

		return window;
	}

	private void HandleRpcErrors()
	{
		foreach (var client in this.rpcClients)
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
