namespace Not.Random;

public static class RandomHelper
{
    static readonly System.Random _random = new();
    static readonly HashSet<int> _uniqueIntegers = [];

    public static int GenerateUniqueInteger()
    {
        lock (_lock)
        {
            var id = _random.Next();
            while (_uniqueIntegers.Contains(id))
            {
                id = _random.Next();
            }

            _uniqueIntegers.Add(id);
            return id;
        }
    }

    static object _lock = new();
}
