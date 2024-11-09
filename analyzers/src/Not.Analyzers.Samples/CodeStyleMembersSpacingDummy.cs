namespace Not.Analyzers.Dummies;

internal class CodeStyleMembersSpacingDummy
{
    const bool PRIVATE_CONST = false;
    static readonly bool PRIVATE_STATIC_READONLY = false;
    public const bool PUBLIC_CONST = true;
    public static readonly bool PUBLIC_STATIC_READONLY = true;

    public static void Test() { }

    bool _privateField;
    protected bool ProtectedField;
    public bool PublicField;

    CodeStyleMembersSpacingDummy() { }

    bool PrivateProperty { get; set; }
    protected bool ProtectedProperty { get; set; }
    public bool PublicProperty { get; set; }

    protected void ProtectedMethod() { }

    public void PublicMethod() { }

    void PrivateMethod() { }

    class PRIVATENESTEDCLASS { }

    protected class ProtectedNestedClass { }

    public class PublicNestedClass { }
}
