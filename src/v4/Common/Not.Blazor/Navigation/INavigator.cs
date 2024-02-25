using Common.Conventions;

namespace Not.Blazor.Navigation;

public interface INavigator : ITransientService
{
    void NavigateTo(string endpoint);
    void NavigateTo<T>(string endpoint, T parameter);
    T ConsumeParameter<T>();
}

