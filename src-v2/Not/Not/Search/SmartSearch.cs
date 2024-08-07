namespace Not.Search;

public class SmartSearch<T>
{
    private readonly List<SearchBase<T>> _searches = new();

    public SmartSearch(IEnumerable<SearchBase<T>> searches)
    {
        _searches = searches.ToList();
    }

    public IEnumerable<T> Find(IEnumerable<T> values, string term)
    {
        var matches = new List<T>();
        foreach (var search in _searches)
        {
            foreach (var result in search.GetMatches(values, term))
            {
                if (!matches.Contains(result))
                {
                    matches.Add(result);
                }
            }
        }
        return matches;
    }
}