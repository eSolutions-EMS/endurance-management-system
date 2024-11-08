using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Not.Analyzers.Base;

public abstract class AnalyzerBase : DiagnosticAnalyzer
{
    protected abstract IEnumerable<DiagnosticDescriptor> CustomRules { get; }
    protected abstract void SafeAnalyzeSyntaxNode(SyntaxNodeAnalysisContext context);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create([ErrorRule, ..CustomRules]);

    protected void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        try
        {
            SafeAnalyzeSyntaxNode(context);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"NoPrivateAnalyzer error: {ex}");
            
            var diagnostic = Diagnostic.Create(ErrorRule, 
                context.Node?.GetLocation(), 
                ex.ToString());
            context.ReportDiagnostic(diagnostic);
        }
    }

    protected readonly DiagnosticDescriptor ErrorRule = new(
        "NA0002",
        "Internal analyzer error",
        "An error occurred in NoPrivateAnalyzer: {0}",
        "Debug",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
}
