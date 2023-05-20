using EMS.Witness.Services;
using Core.Application.Services;
using EMS.Witness.Platforms.Services;

namespace EMS.Witness;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		builder.Services.AddSingleton<ToasterService>();
		builder.Services.AddSingleton<State>();
		builder.Services.AddSingleton<IState>(provider => provider.GetRequiredService<State>());
		builder.Services.AddHttpClient<IApiService, ApiService>(client => client.Timeout = TimeSpan.FromSeconds(5));
		builder.Services.AddTransient<IDateService, DateService>();
		builder.Services.AddTransient<IPermissionsService, PermissionsService>();

		return builder.Build();
	}
}
