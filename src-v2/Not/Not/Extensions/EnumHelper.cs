using System.ComponentModel;
using Not.Reflection;

namespace Not.Extensions;
public static class EnumHelper
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var descriptionAttribute = ReflectionHelper.GetEnumField(type, value)?
                                                   .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                   .FirstOrDefault() as DescriptionAttribute;
        return descriptionAttribute == null ? value.ToString() : descriptionAttribute.Description;
    }
}

