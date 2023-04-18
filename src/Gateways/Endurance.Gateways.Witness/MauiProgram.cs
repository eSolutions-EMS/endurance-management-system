using Microsoft.AspNetCore.Components.WebView.Maui;
using Endurance.Gateways.Witness.Services;

namespace Endurance.Gateways.Witness;

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
		builder.Services.AddSingleton<ContextService>();
		builder.Services.AddSingleton<IContext>(provider => provider.GetRequiredService<ContextService>());
		builder.Services.AddHttpClient<IApiService, ApiService>(client => client.Timeout = TimeSpan.FromSeconds(5));

		return builder.Build();
	}
}
