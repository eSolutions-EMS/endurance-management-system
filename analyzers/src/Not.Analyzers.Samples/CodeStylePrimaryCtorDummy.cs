using System;
using System.Runtime.CompilerServices;

namespace Not.Analyzers.Rules.NoPrimaryConstructor;

public class CodeStylePrimaryCtorDummy
{
    public CodeStylePrimaryCtorDummy(string one, string two)
    {
        One = one;
        Two = two;
    }

    public string One { get; }
    public string Two { get; }
}

public record RecordPrimary(string Test);

public readonly struct StructPrimary
{
    public StructPrimary(string Test)
    {
        this.Test = Test;
    }

    public string Test { get; }
}
