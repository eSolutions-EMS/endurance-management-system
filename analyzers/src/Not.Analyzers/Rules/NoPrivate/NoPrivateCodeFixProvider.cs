using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Not.Analyzers.Base;

namespace Not.Analyzers.Rules.NoPrivate;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NoPrivateCodeFixProvider)), Shared]
public class NoPrivateCodeFixProvider : CodeFixProviderBase
{
    public NoPrivateCodeFixProvider() : base("Remove 'private' keyword", new NoPrivateAnalyzer())
    {
    }

    protected override async Task<Document> SafeCodeFixAction(
        Document document,
        MemberDeclarationSyntax declaration,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var privateModifier = declaration.Modifiers.First(m => m.IsKind(SyntaxKind.PrivateKeyword));
        
        var leadingTrivia = privateModifier.LeadingTrivia;
        
        var leftoverModifiers = declaration.Modifiers.Remove(privateModifier);
        MemberDeclarationSyntax newDeclaration;
        if (leftoverModifiers.Any())
        {
            leftoverModifiers = leftoverModifiers.Replace(
                leftoverModifiers[0], 
                leftoverModifiers[0].WithLeadingTrivia(leadingTrivia));
            newDeclaration = declaration.WithModifiers(leftoverModifiers);
        }
        else
        {
            newDeclaration = declaration.WithModifiers(leftoverModifiers);
            var firstToken = newDeclaration.GetFirstToken();
            newDeclaration = newDeclaration.ReplaceToken(
                firstToken,
                firstToken.WithLeadingTrivia(leadingTrivia));
        }

        editor.ReplaceNode(declaration, newDeclaration);
        return editor.GetChangedDocument();
    }
}
