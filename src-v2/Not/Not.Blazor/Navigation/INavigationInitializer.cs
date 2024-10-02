using Not.Injection;

namespace Not.Blazor.Navigation;

public interface ICrumbsNavigator : ITransientService
{
    void LandTo(string endpoint);
}
