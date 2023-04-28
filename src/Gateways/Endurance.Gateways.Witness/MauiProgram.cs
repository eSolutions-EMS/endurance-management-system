﻿using Microsoft.AspNetCore.Components.WebView.Maui;
using Endurance.Gateways.Witness.Services;
using EnduranceJudge.Application.Services;

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
		builder.Services.AddSingleton<State>();
		builder.Services.AddSingleton<IState>(provider => provider.GetRequiredService<State>());
		builder.Services.AddHttpClient<IApiService, ApiService>(client => client.Timeout = TimeSpan.FromSeconds(5));
		builder.Services.AddTransient<IDateService, DateService>();

		return builder.Build();
	}
}