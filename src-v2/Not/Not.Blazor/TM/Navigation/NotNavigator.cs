using Not.Blazor.Navigation;
using Not.Exceptions;

namespace Not.Blazor.TM.Navigation;

// Cannot be singleton as long as it uses NavigationManager: https://github.com/dotnet/maui/issues/8583
public class NotNavigator : INavigator, ICrumbsNavigator
{
    private readonly NavigationManager _blazorNavigationManager;
    private static Parameters? _parameters;
    private static Stack<(string endpoint, Parameters? parameters)>? _breadCrumbs;

    public NotNavigator(NavigationManager blazorNavigationManager)
    {
        _blazorNavigationManager = blazorNavigationManager;
    }

    public void NavigateTo(string endpoint)
    {
        GuardHelper.ThrowIfDefault(_breadCrumbs);

        _breadCrumbs.Push((endpoint, null));
        NavigateForward(endpoint);
    }

    public void NavigateTo<T>(string endpoint, T parameter)
    {
        GuardHelper.ThrowIfDefault(_breadCrumbs);

        _parameters = Parameters.Create(parameter);
        _breadCrumbs.Push((endpoint, _parameters));
        NavigateForward(endpoint);
    }

    public void NavigateBack()
    {
        GuardHelper.ThrowIfDefault(_breadCrumbs);

        // Pop current
        if (!_breadCrumbs.TryPop(out var _))
        {
            return;
        }
        // Pop previous
        if (!_breadCrumbs.TryPop(out var previousCrumb))
        {
            return;
        }
        _parameters = previousCrumb.parameters;
        NavigateForward(previousCrumb.endpoint);
    }

    public void LandTo(string landing)
    {
        _breadCrumbs = [];
        NavigateTo(landing);
    }

    public T ConsumeParameter<T>()
    {
        if (_parameters == null)
        {
            throw GuardHelper.Exception($"Cannot get parameter '{typeof(T)}'. There are no parameters on this landing");
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
                throw GuardHelper.Exception($"Parameter of type '{typeof(T)}' is null. {nameof(NotNavigator)} parameters cannot be null");
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
                throw GuardHelper.Exception(
                    $"Cannot get parameter of type '{typeof(T)}'/ Underlying type of parameter is '{_parameter.GetType()}'");
            }
            return typedParameter;
        }
    }
}