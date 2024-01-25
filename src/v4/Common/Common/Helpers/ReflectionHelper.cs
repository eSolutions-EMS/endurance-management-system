namespace Common.Utilities;

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
}