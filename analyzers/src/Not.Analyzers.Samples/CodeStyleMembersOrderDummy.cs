using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleMembersOrderDummy(bool NoPrimaryConstructor) : Base
{
    public static void NoPublicStaticAfterCtor() { }

    readonly bool _noPrivateOnField;
    bool _notAssignedUnused;
    bool _noPrivateFieldAfterPublicProperty;
    bool _noPrivateAfterCtor = true;
    public bool noPublicField;

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

    bool NoPrivateOnProperty { get; }
    bool NoPrivatePropertyAfterPublicProperty { get; }
    bool NoPrivatePropertyAfterCtor { get; }
    public bool NoPrimaryConstructor { get; } = NoPrimaryConstructor;

    protected override void NoOverrideBellowPublicMethod() { }

    public void PublicMethod() { }

    public void NoPublicMethodAfterPrivateMethod() { }

    void NoPrivateOnMethod() { }
}

public class EmsCountry : Base
{
    [JsonConstructor]
    EmsCountry() { }
    public EmsCountry(string isoCode, string name, int id)
        : base(id)
    {
        Id = id;
        this.IsoCode = isoCode;
        this.Name = name;
    }

    public string IsoCode { get; private set; }
    public string Name { get; private set; }
}

public class Base
{
    protected Base(int id)
    {
        Id = id;    
    }

    public int Id { get; protected set; }

}
