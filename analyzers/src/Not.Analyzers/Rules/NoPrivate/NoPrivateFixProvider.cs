using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Not.Analyzers.Rules.NoPrivate;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NoPrivateFixProvider)), Shared]
public class NoPrivateFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => [new NoPrivateAnalyzer().DiagnosticId];

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var declaration = root
            ?.FindToken(diagnosticSpan.Start)
            .Parent
            ?.AncestorsAndSelf()
            .OfType<MemberDeclarationSyntax>()
            .First();
        if (declaration == null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Remove 'private' keyword",
                createChangedDocument: c => RemovePrivateKeywordAsync(context.Document, declaration, c),
                equivalenceKey: nameof(NoPrivateFixProvider)),
            diagnostic);
    }

    private async Task<Document> RemovePrivateKeywordAsync(
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
