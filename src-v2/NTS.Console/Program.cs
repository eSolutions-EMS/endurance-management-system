
using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NTS.Judge.MAUI.Server.ACL;
using NTS.Judge.MAUI.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using NTS.Console;

var builder = WebApplication.CreateBuilder();

builder.Services.AddSignalR(x => {
    x.EnableDetailedErrors = true;
});
builder.Services.AddTransient<Service>();

builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Loopback, 11337);
});


var app = builder.Build();
app.Urls.Add("http://*:11337");

Console.WriteLine("Starting Judge server...");

app.MapHub<Kur>("/judge-hub", options =>
{
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents;
});

Task.Run(() => app.RunAsync());
var service = app.Services.GetRequiredService<Service>();

await Task.Delay(TimeSpan.FromSeconds(20));
await service.Ping();

while (true)
{
    Console.Write("Send ping?");
    Console.ReadLine();

    Console.WriteLine("Pinging...");
    await service.Ping();
    Console.WriteLine("Ping complete");
}
