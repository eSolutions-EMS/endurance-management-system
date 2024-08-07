namespace Not.Search;

public class SmartSearchBuilder<T>
{
    private readonly List<SearchBase<T>> _searches = new();

    public SmartSearch<T> Build()
    {
        return new SmartSearch<T>(_searches);
    }

    internal SmartSearchBuilder<T> Add(SearchBase<T> search)
    {
        _searches.Add(search);
        return this;
    }
}

public static class SmartSearchBuilder
{
    public static SmartSearchBuilder<T> For<T>()
    {
        return new SmartSearchBuilder<T>();
    }

    public static SmartSearchBuilder<T> AddString<T>(this SmartSearchBuilder<T> builder, Func<T, string> valueSelector)
    {
        var stringSearch = new StringSearch<T>(valueSelector);
        return builder.Add(stringSearch);
    }

    public static SmartSearchBuilder<T> AddInt<T>(this SmartSearchBuilder<T> builder, Func<T, int> valueSelector)
    {
        var intSearch = new IntSearch<T>(valueSelector);
        return builder.Add(intSearch);
    }
}