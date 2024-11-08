using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Not.Analyzers.Objects;
using static Not.Analyzers.Objects.MemberKind;

namespace Not.Analyzers.Rules.MemberSpacing;

public class MemberSpacingHelper
{
    private static readonly ReadOnlyCollection<MemberKind[]> MemberGroups;
 
    static MemberSpacingHelper()
    {
        var memberGroups = new List<MemberKind[]>
        {
            new[] { PrivateConst, PrivateStaticReadonly, PublicConst, PublicStaticReadonly },
            new[] { PublicStaticMethod },
            new[] { PrivateReadonly, PrivateField, PublicField },
            new[] { PrivateCtor, ProtectedCtor, PublicCtor },
            new[] { AbstractProperty, AbstractMethod },
            new[] { PrivateProperty, ProtectedProperty, InternalProperty, PublicProperty },
            new[] { ProtectedMethod },
            new[] { InternalMethod },
            new[] { PublicMethod },
            new[] { PrivateMethod },
            new[] { ProtectedClass },
            new[] { InternalClass },
            new[] { PrivateClass }
        };

        MemberGroups = new ReadOnlyCollection<MemberKind[]>(memberGroups);
    }
    
    public static bool RequiresBlankLineBetween(MemberKind first, MemberKind second)
    {
        foreach (var group in MemberGroups)
        {
            if (group.Contains(first) && group.Contains(second))
            {
                return false;
            }
        }

        return true;
    }

    public static bool HasLeadingBlankLine(MemberDeclarationSyntax member)
    {
        var trivia = member.GetLeadingTrivia();
        int newlineCount = 0;
        foreach (var t in trivia)
        {
            if (t.IsKind(SyntaxKind.EndOfLineTrivia))
                newlineCount++;
            else if (!t.IsKind(SyntaxKind.WhitespaceTrivia))
                newlineCount = 0;
        }
        return newlineCount >= 1; // One blank line means two newlines
    }
}
