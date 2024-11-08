namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleMembersOrderDummy(bool NoPrimaryConstructor) : Base
{
    bool _noPrivateOnField;
    bool NoPrivateOnProperty { get; }

    public bool noPublicField;
    bool _notAssignedUnused;

    CodeStyleMembersOrderDummy()
        : this(true) // shouldnt have private
    {
        if (_noPrivateFieldAfterPublicProperty) { }
        if (_noPrivateOnField) { }
        if (NoPrivateOnProperty) { }
        if (NoPrivatePropertyAfterPublicProperty) { }
        if (NoPrivatePropertyAfterCtor) { }
        if (_noPrivateAfterCtor) { }
        NoPrivateOnMethod();
        NoPublicStaticAfterCtor();
        PublicMethod();
        NoPublicMethodAfterPrivateMethod();
    }

    public static void NoPublicStaticAfterCtor() { }

    public bool NoPrimaryConstructor { get; } = NoPrimaryConstructor;

    bool NoPrivatePropertyAfterPublicProperty { get; }
    bool NoPrivatePropertyAfterCtor { get; }
    bool _noPrivateFieldAfterPublicProperty;
    bool _noPrivateAfterCtor = true;

    public void PublicMethod() { }

    protected override void NoOverrideBellowPublicMethod() { }

    void NoPrivateOnMethod() { }

    public void NoPublicMethodAfterPrivateMethod() { }
}

public abstract class Base
{
    protected virtual void NoOverrideBellowPublicMethod() { }
}
