using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Not.Analyzers.Objects;

namespace Not.Analyzers.Helpers;

public class MemberKindHelper
{
public static MemberKind GetMemberKind(MemberDeclarationSyntax member)
    {
        return member switch
        {
            FieldDeclarationSyntax field => GetFieldKind(field),
            ConstructorDeclarationSyntax ctor => GetConstructorKind(ctor),
            PropertyDeclarationSyntax prop => GetPropertyKind(prop),
            MethodDeclarationSyntax method => GetMethodKind(method),
            ClassDeclarationSyntax nestedClass => GetNestedClassKind(nestedClass),
            OperatorDeclarationSyntax op => MemberKind.PublicStaticMethod,
            _ => throw new ArgumentException($"Unsupported member type: {member.GetType().Name}")
        };
    }

    private static MemberKind GetFieldKind(FieldDeclarationSyntax field)
    {
        bool isConst = field.Modifiers.Any(SyntaxKind.ConstKeyword);
        bool isStatic = field.Modifiers.Any(SyntaxKind.StaticKeyword);
        bool isReadonly = field.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);
        bool isPrivate = field.Modifiers.Any(SyntaxKind.PrivateKeyword) || !field.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword));
        bool isPublic = field.Modifiers.Any(SyntaxKind.PublicKeyword);

        return (isConst, isStatic, isReadonly, isPrivate, isPublic) switch
        {
            (true, _, _, true, _) => MemberKind.PrivateConst,
            (true, _, _, _, true) => MemberKind.PublicConst,
            (_, true, true, true, _) => MemberKind.PrivateStaticReadonly,
            (_, true, true, _, true) => MemberKind.PublicStaticReadonly,
            (_, _, true, true, _) => MemberKind.PrivateReadonly,
            (_, _, false, true, _) => MemberKind.PrivateField,
            _ => throw new ArgumentException("Unexpected field configuration")
        };
    }

    private static MemberKind GetConstructorKind(ConstructorDeclarationSyntax ctor)
    {
        if (ctor.Modifiers.Any(SyntaxKind.PublicKeyword))
            return MemberKind.PublicCtor;
        if (ctor.Modifiers.Any(SyntaxKind.ProtectedKeyword))
            return MemberKind.ProtectedCtor;
        return MemberKind.PrivateCtor;
    }

    private static MemberKind GetPropertyKind(PropertyDeclarationSyntax prop)
    {
        bool isAbstract = prop.Modifiers.Any(SyntaxKind.AbstractKeyword);
        if (isAbstract)
            return MemberKind.AbstractProperty;

        if (prop.Modifiers.Any(SyntaxKind.PublicKeyword))
            return MemberKind.PublicProperty;
        if (prop.Modifiers.Any(SyntaxKind.ProtectedKeyword))
            return MemberKind.ProtectedProperty;
        return MemberKind.PrivateProperty;
    }

    private static MemberKind GetMethodKind(MethodDeclarationSyntax method)
    {
        bool isAbstract = method.Modifiers.Any(SyntaxKind.AbstractKeyword);
        if (isAbstract)
            return MemberKind.AbstractMethod;

        if (method.Modifiers.Any(SyntaxKind.PublicKeyword))
            return MemberKind.PublicMethod;
        if (method.Modifiers.Any(SyntaxKind.ProtectedKeyword))
            return MemberKind.ProtectedMethod;
        return MemberKind.PrivateMethod;
    }

    private static MemberKind GetNestedClassKind(ClassDeclarationSyntax nestedClass)
    {
        if (nestedClass.Modifiers.Any(SyntaxKind.InternalKeyword))
            return MemberKind.InternalClass;
        return MemberKind.PrivateClass;
    }
}
