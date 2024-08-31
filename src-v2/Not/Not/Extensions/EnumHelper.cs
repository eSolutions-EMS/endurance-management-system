using System.ComponentModel;
using Not.Reflection;

namespace Not.Extensions;

public static class EnumHelper
{
    public static string GetDescription(this Enum value)
    {
        var type = ReflectionHelper.GetType(value);
        var field = ReflectionHelper.GetField(type,value);
        DescriptionAttribute descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                             .FirstOrDefault() as DescriptionAttribute;
        return descriptionAttribute == null ? value.ToString() : descriptionAttribute.Description;
    }
}

