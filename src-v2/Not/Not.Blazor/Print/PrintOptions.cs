using BlazorTemplater;
using System.Linq.Expressions;

namespace Not.Blazor.Print;

public abstract class PrintOptions
{
    public static ComponentRenderOptions<T> Create<T>()
        where T : ComponentBase
    {
        return new ComponentRenderOptions<T>();
    }

    internal abstract MarkupString ToMarkup();

    internal abstract void Use(IServiceProvider provider);
}

public class ComponentRenderOptions<T> : PrintOptions
    where T : ComponentBase
{
    private ComponentRenderer<T> _renderer = new();

    public void Set<TValue>(Expression<Func<T, TValue>> getter, TValue value)
    {
        _renderer.Set(getter, value);
    }

    internal override MarkupString ToMarkup()
    {
        return (MarkupString)_renderer.Render();
    }

    internal override void Use(IServiceProvider provider)
    {
        _renderer.AddServiceProvider(provider);
    }
}
