namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleRestDummy
{
    public static bool operator ==(CodeStyleRestDummy? one, CodeStyleRestDummy? two) => true;
    public static bool operator !=(CodeStyleRestDummy? one, CodeStyleRestDummy? two) => !(one == two);

    bool? _nullableBool;

    public CodeStyleRestDummy()
    {
        var two = Method2();
        var one = Method1();
        ShouldNotAllowNestedInvocations(one, typeof(CodeStyleRestDummy));
    }


    public int? Rest { get; set; } = 15;
    public DateTimeOffset? VetTime { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset? GetOutTime()
    {
        if (Rest == null || VetTime == null)
        {
            return null;
        }
        var b = DateTimeOffset.Now;
        
        VetTime.Value.Add(TimeSpan.FromMinutes(Rest.Value));
        b.Add(TimeSpan.FromMinutes(Rest.Value));
        return VetTime.Value.Add(TimeSpan.FromMinutes(Rest.Value));
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
