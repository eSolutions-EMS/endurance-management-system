using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Not.Analyzers.Base;

public abstract class TypeMemberCodeFixProvider : CodeFixProviderBase
{
    readonly string _analyzerName;
    private readonly string _title;
    private readonly AnalyzerBase _analyzer;

    public TypeMemberCodeFixProvider(string title, AnalyzerBase analyzer) : base(title, analyzer)
    {
        _analyzerName = analyzer.GetType().Name;
        _title = title;
        _analyzer = analyzer;
    }

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

        RegisterCodeFix(context, declaration, diagnostic);
    }
}
