using Not.Injection;

namespace Not.Blazor.Navigation;

public interface INavigationInitializer : ITransientService
{
    void SetLandingPage(string endpoint);
}
