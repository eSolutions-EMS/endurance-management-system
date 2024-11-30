using Not.Reflection;

namespace Not.Injection.Config;

public abstract class NConfig : INConfig
{
    protected abstract string[] RequiredFields { get; }

    void INConfig.Validate()
    {
        throw new ApplicationException($"'{this.GetTypeName()}' configuration is invalid: {string.Join(", ", RequiredFields)} are required");
    }
}

public interface INConfig
{
    internal void Validate();
}
