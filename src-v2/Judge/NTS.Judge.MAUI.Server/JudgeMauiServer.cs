namespace NTS.Judge.MAUI.Server;

public class JudgeMauiServer
{
    public static Task Start(IServiceProvider callerProvider)
    {
        return Task.Run(() => StartServer(callerProvider));
    }

    private static void StartServer(IServiceProvider callerProvider)
    {
        var builder = WebApplication
            .CreateBuilder()
            .AddMauiServerServices(callerProvider);

        var app = builder.Build();

        app.Urls.Add("http://*:11337");

        Console.WriteLine("Starting Judge server...");

        app.Run();
    }
}
