namespace Not.Startup;

/// <summary>
/// Asynchronously execute code at startup. !! The resulted Task is not awaited at application startup !!
/// </summary>
public interface IInitializerAsync
{
    Task Run();
}
