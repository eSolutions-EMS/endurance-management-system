using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleRestDummy
{
    public static void StaticMethod() => Console.WriteLine("test");

    public static IEnumerable<SelectListModel<T>> FromEnum<T>()
        where T : struct, Enum
    {
        var enumValues = Enum.GetValues<T>();
        var selectItems = enumValues.Select(s => new SelectListModel<T>(s, s.GetDescription()));
        var selectItems2 = enumValues.Where(s => Convert((object)s));
        return selectItems;
    }

    public static bool operator ==(CodeStyleRestDummy? one, CodeStyleRestDummy? two) => true;

    public static bool operator !=(CodeStyleRestDummy? one, CodeStyleRestDummy? two) =>
        !(one == two);

    bool? _nullableBool;

    public CodeStyleRestDummy()
    {
        var instances = new List<Instance> { new Instance(), new Instance() };

        var a = instances.Select(x => x.ConvertToInt());
        var b = instances.Select(x => x.ConvertToInt().ToString());

        var two = Method2();
        var one = Method1();
        ShouldNotAllowNestedInvocations(one, typeof(CodeStyleRestDummy));
    }

    public int? Rest { get; set; } = 15;
    public DateTimeOffset? VetTime { get; set; } = DateTimeOffset.Now;

    public void Method() => Console.WriteLine("test");

    public DateTimeOffset? GetOutTime()
    {
        if (Rest == null || VetTime == null)
        {
            return null;
        }
        var timeSpan = TimeSpan.FromMinutes(Rest.Value);
        var b = VetTime?.Add(timeSpan);
        ;

        return b;
    }

    void ShouldNotAllowNestedInvocations(bool one, Type two) { }

    bool Method1()
    {
        return true;
    }

    bool Method2()
    {
        return false;
    }

    object Convert(object obj)
    {
        return obj;
    }

    void EnforceSingleLineBraces()
    {
        if (true)
            Console.WriteLine();
        if (true)
            Console.WriteLine();
    }
}

public static class Extensions
{
    public static bool Flip(this bool value)
    {
        return !value;
    }
}

public class Instance
{
    public int ConvertToInt()
    {
        return 5;
    }
}

public class SelectListModel<T>
{
    public SelectListModel(T value, string description)
    {
        Value = value;
        Description = description;
    }

    public T Value { get; set; }
    public string Description { get; set; }
}

public static class EnumHelper
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var descriptionAttribute =
            GetEnumField(type, value)
                ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;
        return descriptionAttribute == null ? value.ToString() : descriptionAttribute.Description;
    }

    public static FieldInfo? GetEnumField(Type type, object instance)
    {
        var stringValue = instance.ToString();
        if (stringValue == null)
            return null;
        return type.GetField(stringValue);
    }
}
