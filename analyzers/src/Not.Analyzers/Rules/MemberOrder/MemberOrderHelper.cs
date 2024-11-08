using System.Collections.Immutable;
using Not.Analyzers.Objects;

namespace Not.Analyzers.Rules.MemberOrder;

public static class MemberOrderHelper
{
    static readonly ImmutableDictionary<MemberKind, int> _orderByMemberKind;

    static MemberOrderHelper()
    {
        // Initialize ordering - lower number means higher in file
        var ordering = new Dictionary<MemberKind, int>();
        var order = 0;
        
        // Constants and Static Readonly Fields
        ordering[MemberKind.PrivateConst] = order++;
        ordering[MemberKind.PrivateStaticReadonly] = order++;
        ordering[MemberKind.PublicConst] = order++;
        ordering[MemberKind.PublicStaticReadonly] = order++;

        // Instance Fields
        ordering[MemberKind.PrivateReadonly] = order++;
        ordering[MemberKind.PrivateField] = order++;

        // Constructors
        ordering[MemberKind.PrivateCtor] = order++;
        ordering[MemberKind.ProtectedCtor] = order++;
        ordering[MemberKind.PublicCtor] = order++;

        // Abstract Members
        ordering[MemberKind.AbstractProperty] = order++;
        ordering[MemberKind.AbstractMethod] = order++;

        // Properties
        ordering[MemberKind.PrivateProperty] = order++;
        ordering[MemberKind.ProtectedProperty] = order++;
        ordering[MemberKind.PublicProperty] = order++;

        // Methods
        ordering[MemberKind.ProtectedMethod] = order++;
        ordering[MemberKind.PublicMethod] = order++;
        ordering[MemberKind.PrivateMethod] = order++;

        // Nested Types
        ordering[MemberKind.InternalClass] = order++;
        ordering[MemberKind.PrivateClass] = order++;

        _orderByMemberKind = ordering.ToImmutableDictionary();
    }

    public static bool ShouldComeBefore(MemberKind first, MemberKind second)
        => _orderByMemberKind[first] < _orderByMemberKind[second];
} 
