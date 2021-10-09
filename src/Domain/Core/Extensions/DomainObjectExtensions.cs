using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings.Domain;

namespace EnduranceJudge.Domain.Core.Extensions
{
    public static class DomainObjectExtensions
    {
        public static void RemoveObject<T>(this ICollection<T> collection, T model)
            where T : IDomainObject
        {
            if (model == null)
            {
                throw new DomainException(CannotRemoveNullItemTemplate);
            }

            if (!collection.Contains(model))
            {
                throw new DomainException(CannotRemoveItemIsNotFoundTemplate, model.GetType().Name);
            }

            collection.Remove(model);
        }

        public static void AddObject<T>(this ICollection<T> collection, T model)
            where T : IDomainObject
        {
            if (model == null)
            {
                throw new DomainException(CannotAddNullItemTemplate);
            }

            if (collection.Contains(model))
            {
                throw new DomainException(CannotAddItemExistsTemplate, model.GetType().Name);
            }

            collection.Add(model);
        }

        public static void  AddOrUpdateObject<T>(this ICollection<T> collection, T model)
            where T : IDomainObject
        {
            if (model == null)
            {
                throw new DomainException(CannotAddNullItemTemplate);
            }

            if (collection.Contains(model))
            {
                collection.UpdateObject(model);
                return;
            }

            collection.Add(model);
        }

        public static void UpdateObject<T>(this ICollection<T> collection, T model)
            where T : IDomainObject
        {
            if (model == null)
            {
                throw new DomainException(CannotAddNullItemTemplate);
            }

            if (!collection.Contains(model))
            {
                return;
            }

            var item = collection.First(x => x.Equals(model));
            if (!ReferenceEquals(item, model))
            {
                item.MapFrom(model);
            }
        }
    }
}
