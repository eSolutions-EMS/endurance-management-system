using EMS.Judge.Application.Common.Models;
using Core.Mappings;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core.Localization.Strings;

namespace EMS.Judge.Common.Components.Templates.SimpleListItem;

public class SimpleListItemViewModel : IMapFrom<ListItemModel>
{
    public SimpleListItemViewModel()
    {
    }

    public SimpleListItemViewModel(IListable listable)
    {
        this.Id = listable.Id;
        this.Name = listable.Name;
    }

    public SimpleListItemViewModel(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    public static IEnumerable<SimpleListItemViewModel> FromEnum<T>()
        where T : struct, Enum
    {
        var names = Enum.GetNames<T>();
        var values = Enum
            .GetValues<T>()
            .Cast<int>()
            .ToArray();

        for (var i = 1; i < names.Length; i++)
        {
            var name = names[i];
            var value = values[i];

            yield return new SimpleListItemViewModel(value, name);
        }
    }

    public static List<SimpleListItemViewModel> FromBool()
    {
        var falseItem = new SimpleListItemViewModel(0, NO);
        var trueItem = new SimpleListItemViewModel(1, YES);

        var result =  new List<SimpleListItemViewModel> { falseItem, trueItem };

        return result;
    }
}
