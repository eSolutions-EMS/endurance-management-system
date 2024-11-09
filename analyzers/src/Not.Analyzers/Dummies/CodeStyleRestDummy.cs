namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleRestDummy
{
    bool? _nullableBool;

    public CodeStyleRestDummy()
    {
        ShouldNotAllowNestedInvocations(Method1(), Method2());
        ShouldNotAllowNestedInvocations(Method1(), Method2().Flip());

        var one = Method1();
        ShouldNotAllowNestedInvocations(one, Method2());

        var two = Method2();
        ShouldNotAllowNestedInvocations(Method1(), two);
    }

    void ShouldNotAllowNestedInvocations(bool one, bool two)
    {
    }

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
        if (true) Console.WriteLine();
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

