using EMS.Witness.Services;

namespace EMS.Witness;

public partial class App : Application
{
	private readonly IApiService apiService;

	public App(IApiService apiService)
	{
		InitializeComponent();

		MainPage = new MainPage();
		this.apiService = apiService;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = base.CreateWindow(activationState);

		window.Activated += async (s, e) => await this.apiService.FetchInitialState();

		return window;
	}
}
