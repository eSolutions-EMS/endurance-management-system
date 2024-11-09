using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Not.Analyzers.Base;

namespace Not.Analyzers.Rules.NoPrivate;

public abstract class TypeMemberCodeFixProviderBase : CodeFixProvider
{
    private readonly DiagnosticDescriptor _errorRule;
    private readonly string _title;
    private readonly AnalyzerBase _analyzer;

    public TypeMemberCodeFixProviderBase(string title, AnalyzerBase analyzer)
    {
        _errorRule = new(
            "NA0000",
            "Internal code fix provider error",
            "An error occurred in code fix provider: {0}",
            "Debug",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
        _title = title;
        _analyzer = analyzer;
    }

    public sealed override ImmutableArray<string> FixableDiagnosticIds => [_analyzer.DiagnosticId];

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    protected abstract Task<Document> SafeCodeFixAction(
        Document document,
        MemberDeclarationSyntax declaration,
        CancellationToken cancellationToken);

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
                title: _title,
                createChangedDocument: c => SafeCodeFixAction(context.Document, declaration, c),
                equivalenceKey: nameof(NoPrivateCodeFixProvider)),
            diagnostic);
    }

    private async Task<Document> RemovePrivateKeywordAsync(
        Document document,
        MemberDeclarationSyntax declaration,
        CancellationToken cancellationToken)
    {
        try 
        {
            return await SafeCodeFixAction(document, declaration, cancellationToken);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Code fixer {GetType().Name} error: {ex}");
            return document;
        }
    }
}
