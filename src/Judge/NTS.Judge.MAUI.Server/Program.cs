using NTS.Application;
using NTS.Judge.MAUI.Server.ACL;
using NTS.Judge.MAUI.Server.RPC;
using NTS.Judge.MAUI.Server;

var builder = WebApplication.CreateBuilder();

builder.Services.ConfigureHub();

var app = builder.Build();

app.Urls.Add("http://*:11337");

app.MapHub<JudgeRpcHub>(ApplicationConstants.JUDGE_HUB);
app.MapHub<WitnessRpcHub>(Constants.RPC_ENDPOINT); // TODO: change to NtsApplicationConstants.WITNESS_HUB

app.Run();
