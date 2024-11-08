using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Not.Analyzers.Base;
using Not.Analyzers.Members;
using Not.Analyzers.Rules.NoPrivate;

namespace Not.Analyzers.Rules.MemberOrder;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MemberOrderCodeFixProvider)), Shared]
public class MemberOrderCodeFixProvider : CodeFixProviderBase
{
    public MemberOrderCodeFixProvider() : base("Fix member ordering", new MemberOrderAnalyzer())
    {
    }

    protected override async Task<Document> SafeCodeFixAction(
        Document document,
        MemberDeclarationSyntax declaration,
        CancellationToken cancellationToken)
    {
        if (declaration is not TypeDeclarationSyntax typeDeclaration)
        {
            return document;
        }

        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        
        var orderedMembers = typeDeclaration.Members
            .OrderBy(MemberKindHelper.GetMemberKind)
            .ToList();

        var newTypeDeclaration = typeDeclaration.WithMembers(SyntaxFactory.List(orderedMembers));
        editor.ReplaceNode(typeDeclaration, newTypeDeclaration);
        
        return editor.GetChangedDocument();
    }
} 
