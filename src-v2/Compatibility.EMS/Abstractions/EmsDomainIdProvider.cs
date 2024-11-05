namespace NTS.Compatibility.EMS.Abstractions;

public static class EmsDomainIdProvider
{
    private static readonly Random Random = new();
    private static readonly HashSet<int> DomainIds = [];

    public static int Generate()
    {
        var id = Random.Next();
        while (DomainIds.Contains(id))
        {
            id = Random.Next();
        }

        DomainIds.Add(id);
        return id;
    }
}
