using System;
using System.Collections.Generic;

namespace EnduranceJudge.Domain.Core;

public static class DomainIdProvider
{
    private static readonly Random Random = new();
    private static readonly HashSet<int> DomainIds = new();

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