using EnduranceJudge.Core.Exceptions;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Models;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings.DomainStrings;

namespace EnduranceJudge.Core.Extensions
{
    public static class IObjectExtensions
    {
        public static void RemoveObject<T>(this ICollection<T> collection, T model)
            where T : IObject
        {
            if (model == null)
            {
                throw new CoreException(CannotRemoveNullItemTemplate);
            }

            if (!collection.Contains(model))
            {
                throw new CoreException(CannotRemoveItemIsNotFoundTemplate, model.GetType().Name);
            }

            collection.Remove(model);
        }

        public static void AddObject<T>(this ICollection<T> collection, T model)
            where T : IObject
        {
            if (model == null)
            {
                throw new CoreException(CannotAddNullItemTemplate);
            }

            if (collection.Contains(model))
            {
                throw new CoreException(CannotAddItemExistsTemplate, model.GetType().Name);
            }

            collection.Add(model);
        }

        public static void AddOrUpdateObject<T>(this ICollection<T> collection, T model)
            where T : IObject
        {
            if (model == null)
            {
                throw new CoreException(CannotAddNullItemTemplate);
            }

            if (collection.Contains(model))
            {
                var item = collection.First(x => x.Equals(model));
                item.MapFrom(model);

                return;
            }

            collection.Add(model);
        }
    }
}
