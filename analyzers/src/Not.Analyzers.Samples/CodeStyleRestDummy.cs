namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleRestDummy
{
    bool? _nullableBool;

    public CodeStyleRestDummy()
    {
        var two = Method2();
        var one = Method1();
        ShouldNotAllowNestedInvocations(one, two);
    }

    void ShouldNotAllowNestedInvocations(bool one, bool two) { }

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
