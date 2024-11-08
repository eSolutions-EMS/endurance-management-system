using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Not.Analyzers.Base;
using Not.Analyzers.Members;
using Not.Analyzers.Rules.NoPrivate;

namespace Not.Analyzers.Rules.MemberSpacing;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MemberSpacingCodeFixProvider)), Shared]
public class MemberSpacingCodeFixProvider : CodeFixProviderBase
{
    public MemberSpacingCodeFixProvider() : base("Fix member spacing", new MemberSpacingAnalyzer())
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
        
        var members = typeDeclaration.Members.ToList();
        for (int i = 1; i < members.Count; i++)
        {
            var currentMember = members[i - 1];
            var nextMember = members[i];
            
            var currentKind = MemberKindHelper.GetMemberKind(currentMember);
            var nextKind = MemberKindHelper.GetMemberKind(nextMember);

            if (MemberSpacingHelper.RequiresBlankLineBetween(currentKind, nextKind))
            {
                members[i] = nextMember
                    .WithLeadingTrivia(SyntaxFactory.EndOfLine("\n"))
                    .WithLeadingTrivia(nextMember.GetLeadingTrivia());
            }
        }

        var newTypeDeclaration = typeDeclaration.WithMembers(SyntaxFactory.List(members));
        editor.ReplaceNode(typeDeclaration, newTypeDeclaration);
        
        return editor.GetChangedDocument();
    }
} 
