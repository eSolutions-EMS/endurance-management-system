using System.Reflection;

namespace Not.Utilities;

public static class ReflectionHelper
{
    public static Type GetRootType(this Type type)
    {
        var parentType = type.BaseType;
        while (parentType != null)
        {
            parentType = parentType.BaseType;
        }
        return parentType ?? type;
    }

    public static string GetName<T>()
    {
        return typeof(T).Name;
    }

    public static PropertyInfo Property(this Type type, string name)
    {
        return type.GetProperties().FirstOrDefault(x => x.Name == name)
            ?? throw new Exception($"Property '{name}' does not exist on type '{type.Name}'");
    }

    public static MethodInfo Method(this Type type, string name)
    {
        return type.GetMethod(name)
            ?? throw new Exception($"'{type}' doesn't have method method '{name}'");
    }

    public static object? Get(this PropertyInfo property, object instance)
    {
        return property.GetValue(instance);
    }

    public static void Set(this PropertyInfo property, object instance, object value)
    {
        property.SetValue(instance, value);
    }
}