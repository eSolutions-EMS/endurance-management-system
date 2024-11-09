using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Not.Analyzers.Base;

namespace Not.Analyzers.Rules.NoNestedInvocation;

[ExportCodeFixProvider(LanguageNames.CSharp)]
public class NestedInvocationCodeFixProvider : CodeFixProviderBase
{
    public NestedInvocationCodeFixProvider()
        : base("Extract nested invocations", NestedInvocationAnalyzer.RULE_ID) { }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context
            .Document.GetSyntaxRootAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        if (root?.FindNode(diagnosticSpan) is InvocationExpressionSyntax invocation)
        {
            RegisterCodeFix(context, invocation, diagnostic);
        }
    }

    protected override async Task<Document> SafeCodeFixAction(
        Document document,
        CSharpSyntaxNode node,
        CancellationToken cancellationToken
    )
    {
        var invocation = (InvocationExpressionSyntax)node;
        var root = await document.GetSyntaxRootAsync(cancellationToken);

        // Find the parent statement
        var statement = invocation.Ancestors().OfType<StatementSyntax>().First();
        if (statement.Parent is not BlockSyntax block)
        {
            return document;
        }

        // Extract nested invocations
        var newStatements = await ExtractNestedInvocations(invocation, document, cancellationToken);

        // Replace the original statement with new statements
        var newBlock = block.ReplaceNode(statement, newStatements);
        var newRoot = root!.ReplaceNode(block, newBlock);

        return document.WithSyntaxRoot(newRoot);
    }

    private async Task<List<StatementSyntax>> ExtractNestedInvocations(
        InvocationExpressionSyntax invocation,
        Document document,
        CancellationToken cancellationToken
    )
    {
        var statements = new List<StatementSyntax>();
        var tempCount = 0;

        // Get method symbol to access parameter names
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
        var methodSymbol = semanticModel?.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        var parameters = methodSymbol?.Parameters;

        var updatedArgs = invocation
            .ArgumentList.Arguments.Select(
                (arg, index) =>
                {
                    // Check if argument is directly a method invocation or contains nested invocations
                    if (
                        arg.Expression is not InvocationExpressionSyntax
                        && !arg
                            .Expression.DescendantNodes()
                            .OfType<InvocationExpressionSyntax>()
                            .Any()
                    )
                    {
                        return arg;
                    }

                    // Get parameter name or fallback to a generic name
                    var baseName = "temp";
                    if (parameters != null && index < parameters.Value.Length)
                    {
                        baseName = parameters.Value[index].Name;
                    }
                    else if (arg.NameColon != null)
                    {
                        baseName = arg.NameColon.Name.Identifier.Text;
                    }

                    // Ensure unique name by adding number if needed
                    var tempName = baseName;
                    while (statements.Any(s => ContainsVariableName(s, tempName)))
                    {
                        tempName = $"{baseName}{++tempCount}";
                    }

                    // Add variable declaration
                    var declaration = SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("var"),
                            SyntaxFactory.SeparatedList(
                                [
                                    SyntaxFactory
                                        .VariableDeclarator(tempName)
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(arg.Expression)
                                        ),
                                ]
                            )
                        )
                    );

                    statements.Add(declaration);

                    // Replace argument with temp variable
                    return arg.WithExpression(SyntaxFactory.IdentifierName(tempName));
                }
            )
            .ToArray();

        var updatedInvocation = invocation.WithArgumentList(
            SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(updatedArgs))
        );

        statements.Add(SyntaxFactory.ExpressionStatement(updatedInvocation));

        return statements;
    }

    private bool ContainsVariableName(StatementSyntax statement, string name)
    {
        if (statement is LocalDeclarationStatementSyntax localDeclaration)
        {
            return localDeclaration.Declaration.Variables.Any(v => v.Identifier.Text == name);
        }
        return false;
    }
}
