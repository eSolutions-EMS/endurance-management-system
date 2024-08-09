using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Not.Blazor.TM.Models;

public class MultiSelectListModel
{
    public MultiSelectListModel(string value, string description)
    {
        Value = value;
        Description = description;
    }

    public string Value { get; set; }
    public string Description { get; set; }

    public static IEnumerable<MultiSelectListModel> MultiSelectList<T>()
    {   
        var enumValues = Enum.GetValues(typeof(T)).Cast<Enum>();
        var selectItems = enumValues.Select(s => new MultiSelectListModel(s.ToString(), EnumHelper.GetDescription(s)));
        return selectItems;
    }
}

public static class EnumHelper
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
        DescriptionAttribute descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                             .FirstOrDefault() as DescriptionAttribute;
        return descriptionAttribute == null ? value.ToString() : descriptionAttribute.Description;
    }
}

