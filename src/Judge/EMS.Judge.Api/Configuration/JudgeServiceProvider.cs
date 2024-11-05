using System;

namespace EMS.Judge.Api.Configuration;

public class JudgeServiceProvider : IJudgeServiceProvider
{
    private readonly IServiceProvider _innerProvider;

    public JudgeServiceProvider(IServiceProvider innerProvider)
    {
        this._innerProvider = innerProvider;
    }

    public object GetService(Type serviceType) => this._innerProvider.GetService(serviceType);
}

/// <summary>
/// Contains an instance if "IServiceProvider" from Judge app.
/// This is necessary because ASP.NET manages it's own container which is still necessary
/// in order for it to configure itself properly
/// The end result is that from within this API any Judge services (services that modify app state)
/// have to be retrieved using "IJudgeServiceProvider" as they are not registered
/// </summary>
public interface IJudgeServiceProvider : IServiceProvider { }
