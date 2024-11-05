namespace Not.Random;

public static class RandomHelper
{
    private static object _lock = new();
    private static readonly System.Random Random = new();
    private static readonly HashSet<int> UniqueIntegers = [];

    public static int GenerateUniqueInteger()
    {
        lock (_lock)
        {
            var id = Random.Next();
            while (UniqueIntegers.Contains(id))
            {
                id = Random.Next();
            }

            UniqueIntegers.Add(id);
            return id;
        }
    }
}
