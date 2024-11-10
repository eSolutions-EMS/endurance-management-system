using System.Collections.Immutable;
using Not.Analyzers.Members;

namespace Not.Analyzers.Rules.MemberOrder;

public static class MemberOrderHelper
{
    static readonly ImmutableDictionary<MemberKind, int> _orderByMemberKind;

    static MemberOrderHelper()
    {
        // Initialize ordering - lower number means higher in file
        var ordering = new Dictionary<MemberKind, int>();
        var order = 0;

        ordering[MemberKind.Delegate] = order++;
        
        // Constants and Static Readonly Fields
        ordering[MemberKind.PrivateConst] = order++;
        ordering[MemberKind.PrivateStaticReadonly] = order++;
        ordering[MemberKind.PublicConst] = order++;
        ordering[MemberKind.PublicStaticReadonly] = order++;
        ordering[MemberKind.PublicStaticEvent] = order++;

        ordering[MemberKind.PublicStaticMethod] = order++;
        ordering[MemberKind.PublicImplicitOperator] = order++;
        ordering[MemberKind.PublicOperator] = order++;

        // Instance Fields
        ordering[MemberKind.PrivateReadonly] = order++;
        ordering[MemberKind.PrivateField] = order++;
        ordering[MemberKind.PrivateEvent] = order++;
        ordering[MemberKind.PublicField] = order++;

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
        ordering[MemberKind.PublicIndexDeclarator] = order++;
        ordering[MemberKind.PublicEvent] = order++;
        ordering[MemberKind.PublicProperty] = order++;

        // Methods
        ordering[MemberKind.ProtectedMethod] = order++;
        ordering[MemberKind.InternalMethod] = order++;
        ordering[MemberKind.PublicMethod] = order++;
        ordering[MemberKind.PrivateMethod] = order++;

        // Nested Types
        ordering[MemberKind.InternalClass] = order++;
        ordering[MemberKind.PrivateClass] = order++;

        _orderByMemberKind = ordering.ToImmutableDictionary();
    }

    public static int CompareOrder(MemberKind first, MemberKind second)
    {
        if (!_orderByMemberKind.TryGetValue(first, out var firstOrder))
        {
            throw new Exception($"Unknown member kind: {first}");
        }
        if (!_orderByMemberKind.TryGetValue(second, out var secondOrder))
        {
            throw new Exception($"Unknown member kind: {second}");
        }
        return Comparer<int>.Default.Compare(firstOrder, secondOrder);
    }
}
