using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Not.Analyzers.Base;

namespace Not.Analyzers.Rules.NoNestedInvocation;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NestedInvocationAnalyzer : AnalyzerBase
{
    public const string RULE_ID = "NA0006";
    private const string TITLE = "Avoid nested method invocations";
    private const string MESSAGE_FORMAT = "Nested method invocations reduce code readability";
    private const string DESCRIPTION =
        "Method invocations should be separated into multiple statements for better readability.";

    public NestedInvocationAnalyzer()
        : base(RULE_ID, TITLE, MESSAGE_FORMAT, description: DESCRIPTION) { }

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InvocationExpression);
    }

    protected override void SafeAnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        foreach (var argument in invocation.ArgumentList.Arguments)
        {
            if (
                argument.Expression is InvocationExpressionSyntax
                || argument.Expression.DescendantNodes().OfType<InvocationExpressionSyntax>().Any()
            )
            {
                context.ReportDiagnostic(CreateDiagnostic(invocation.GetLocation()));
                break;
            }
        }
    }
}
