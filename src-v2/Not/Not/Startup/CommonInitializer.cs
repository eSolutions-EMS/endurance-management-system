namespace Not.Startup;

public class CommonInitializer : IInitializer
{
    private readonly IServiceProvider _provider;

    public CommonInitializer(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void Run()
    {
        ServiceLocator.Initialize(_provider);
    }
}
