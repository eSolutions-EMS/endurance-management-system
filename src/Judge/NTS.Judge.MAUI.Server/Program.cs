using NTS.ACL.Handshake;
using NTS.Application;
using Not.Injection;
using NTS.Judge.MAUI.Server.ACL;
using NTS.Storage.Injection;
using NTS.Judge.MAUI.Server.RPC;

var builder = WebApplication.CreateBuilder();
builder.Services.AddSignalR();
builder.Services.AddHostedService<NetworkBroadcastService>();
builder.Services.GetConventionalAssemblies().RegisterConventionalServices();
builder.Services.AddStorage();

var app = builder.Build();

app.Urls.Add("http://*:11337");

app.MapHub<JudgeRpcHub>(NtsApplicationConstants.JUDGE_HUB);
app.MapHub<WitnessRpcHub>(Constants.RPC_ENDPOINT); // TODO: change to NtsApplicationConstants.WITNESS_HUB

app.Run();
