using System.Collections.Immutable;
using Not.Analyzers.Objects;

namespace Not.Analyzers.Rules.MemberSpacing;

public class MemberSpacingHelper
{
    private static readonly ImmutableHashSet<(MemberKind First, MemberKind Second)> RequiresBlankLine;
 
    static MemberSpacingHelper()
    {
        var blankLines = new HashSet<(MemberKind, MemberKind)>
        {
            (MemberKind.PublicStaticReadonly, MemberKind.PrivateReadonly),
            (MemberKind.PrivateReadonly, MemberKind.PrivateField),
            (MemberKind.PrivateField, MemberKind.PrivateCtor),
            (MemberKind.PublicCtor, MemberKind.AbstractProperty),
            (MemberKind.AbstractMethod, MemberKind.PrivateProperty),
            (MemberKind.PublicProperty, MemberKind.ProtectedMethod),
            (MemberKind.PublicMethod, MemberKind.PrivateMethod),
            (MemberKind.PrivateMethod, MemberKind.InternalClass),
        };

        RequiresBlankLine = blankLines.ToImmutableHashSet();
    }
    
    public static bool RequiresBlankLineBetween(MemberKind first, MemberKind second)
        => RequiresBlankLine.Contains((first, second));
}
