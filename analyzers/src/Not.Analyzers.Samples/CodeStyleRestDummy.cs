namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleRestDummy
{
    public static bool operator ==(CodeStyleRestDummy? one, CodeStyleRestDummy? two) => true;

    public static bool operator !=(CodeStyleRestDummy? one, CodeStyleRestDummy? two) => !(one == two);

    public static void StaticMethod() => Console.WriteLine("test");

    bool? _nullableBool;

    public CodeStyleRestDummy()
    {
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
        var b = VetTime?.Add(timeSpan); ;


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
