using System;
using System.Runtime.CompilerServices;

namespace Not.Analyzers.Rules.NoPrimaryConstructor;

public class CodeStylePrimaryCtorDummy
{
    public string One { get; }

    public string Two { get; }

    public CodeStylePrimaryCtorDummy(string one, string two)
    {
        One = one;
        Two = two;
    }
}

public record RecordPrimary
{
    public string Test { get; }

    public RecordPrimary(string test)
    {
        Test = test;
    }
};

public readonly struct StructPrimary
{
    public string Test { get; }

    public StructPrimary(string Test)
    {
        this.Test = Test;
    }
}
