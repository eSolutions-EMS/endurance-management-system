namespace Not.Random;

public static class RandomHelper
{
    private static readonly System.Random Random = new();
    private static readonly HashSet<int> UniqueIntegers = new();

    public static int GenerateUniqueInteger()
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
