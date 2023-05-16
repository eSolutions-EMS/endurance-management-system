using EMS.Core.Domain.Core.Models;
using EMS.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Core.Domain.Core.Extensions;

public static class DomainExtensions
{
    public static void AddOrUpdate<T>(this ICollection<T> collection, T domainObject)
        where T : IDomain
    {
        if (domainObject == null)
        {
            throw new ArgumentNullException(nameof(domainObject));
        }
        if (collection.Contains(domainObject))
        {
            var match = collection.MatchDomain(domainObject);
            match.MapFrom(domainObject);
            return;
        }
        collection.Add(domainObject);
    }

    public static T MatchDomain<T>(this IEnumerable<T> collection, T domainObject)
        where T : IDomain
    {
        var result = collection.FirstOrDefault(x => x.Id == domainObject.Id);
        return result;
    }

    public static T FindDomain<T>(this IEnumerable<T> collection, int id)
        where T : IDomain
    {
        var result = collection.FirstOrDefault(x => x.Id == id);
        return result;
    }
}
