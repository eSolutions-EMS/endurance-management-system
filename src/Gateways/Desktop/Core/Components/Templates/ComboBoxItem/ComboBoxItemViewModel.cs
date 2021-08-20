using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ComboBoxItem
{
    public class ComboBoxItemViewModel : IMapFrom<ListItemModel>
    {
        public ComboBoxItemViewModel()
        {
        }

        public ComboBoxItemViewModel(IListable listable)
        {
            this.Id = listable.Id;
            this.Name = listable.Name;
        }

        public ComboBoxItemViewModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; }

        public string Name { get; }

        public static IEnumerable<ComboBoxItemViewModel> FromEnum<T>()
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

                yield return new ComboBoxItemViewModel(value, name);
            }
        }

        public static List<ComboBoxItemViewModel> FromBool()
        {
            var falseItem = new ComboBoxItemViewModel(0, DesktopStrings.BoolFalseValue);
            var trueItem = new ComboBoxItemViewModel(1, DesktopStrings.BoolTrueValue);

            var result =  new List<ComboBoxItemViewModel> { falseItem, trueItem };

            return result;
        }
    }
}
