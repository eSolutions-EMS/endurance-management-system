
using Not.Helpers;
using Microsoft.AspNetCore.Components;
using Not.Blazor.Navigation;

namespace Not.Blazor.TM.Navigation;

// Cannot be singleton as long as it uses NavigationManager: https://github.com/dotnet/maui/issues/8583
public class NotNavigator : INavigator
{
    private readonly NavigationManager _blazorNavigationManager;
    private static Parameters? _parameters;

    public NotNavigator(NavigationManager blazorNavigationManager)
    {
        _blazorNavigationManager = blazorNavigationManager;
    }

    public void NavigateTo(string endpoint)
    {
        NavigateForward(endpoint);
    }

    public void NavigateTo<T>(string endpoint, T parameter)
    {
        _parameters = Parameters.Create(parameter);
        NavigateForward(endpoint);
    }

    public T ConsumeParameter<T>()
    {
        if (_parameters == null)
        {
            throw ThrowHelper.Exception($"Cannot get parameter '{typeof(T)}'. There are no parameters on this landing");
        }
        var result = _parameters.Get<T>();
        _parameters = null;
        return result;
    }

    private void NavigateForward(string endpoint)
    {
        _blazorNavigationManager.NavigateTo(endpoint);
    }

    internal class Parameters
    {
        private readonly object _parameter;

        public static Parameters Create<T>(T parameter)
        {
            if (parameter == null)
            {
                throw ThrowHelper.Exception($"Parameter of type '{typeof(T)}' is null. {nameof(NotNavigator)} parameters cannot be null");
            }
            return new Parameters(parameter);
        }

        private Parameters(object parameter)
        {
            _parameter = parameter;
        }

        public T Get<T>()
        {
            if (_parameter is not T typedParameter)
            {
                throw ThrowHelper.Exception(
                    $"Cannot get parameter of type '{typeof(T)}'/ Underlying type of parameter is '{_parameter.GetType()}'");
            }
            return typedParameter;
        }
    }
}