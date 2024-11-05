namespace Not.Search;

internal abstract class PropertySearch<T, TValue> : SearchBase<T>
{
    private readonly Func<T, TValue?> _selector;

    protected PropertySearch(Func<T, TValue?> selector)
    {
        _selector = selector;
    }

    public override IEnumerable<T> GetMatches(IEnumerable<T> instances, string value)
    {
        // Try-catch because it is not possible to define the return type of ConvertSearch as nullable
        // and use null to check if the term is valid for the given search
        // see more: https://stackoverflow.com/questions/71716403/override-abstract-method-as-nullable-both-reference-and-value-type
        try
        {
            var result = new List<T>();
            var term = ConvertTerm(value);
            foreach (var instance in instances)
            {
                if (IsMatch(_selector(instance), term))
                {
                    result.Add(instance);
                }
            }
            return result;
        }
        catch (PropertyTermException)
        {
            return Enumerable.Empty<T>();
        }
    }

    protected virtual bool IsMatch(TValue? selected, TValue search)
    {
        return EqualityComparer<TValue?>.Default.Equals(selected, search);
    }

    protected abstract TValue ConvertTerm(string term);
}

internal class StringPropertySearch<T> : PropertySearch<T, string>
{
    public StringPropertySearch(Func<T, string> selector)
        : base(selector) { }

    protected override string ConvertTerm(string value)
    {
        return value;
    }

    protected override bool IsMatch(string? selected, string term)
    {
        return selected?.Contains(term) ?? false;
    }
}

internal class IntPropertySearch<T> : PropertySearch<T, int>
{
    public IntPropertySearch(Func<T, int> selector)
        : base(selector) { }

    protected override int ConvertTerm(string value)
    {
        if (!int.TryParse(value, out int search))
        {
            throw new PropertyTermException(
                $"Search term '{value}' cannot be parsed to a valid integer"
            );
        }
        return search;
    }
}
