using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Not.Analyzers.Base;
using Not.Analyzers.Helpers;

namespace Not.Analyzers.Rules.MemberSpacing;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MemberSpacingAnalyzer : AnalyzerBase
{
    public MemberSpacingAnalyzer() : base(
        diagnosticId: "NA0004",
        title: "Member spacing rule violation",
        messageFormat: "A blank line is required between {0} and {1}",
        description: "Members should be separated by blank lines according to the specified convention.")
    {
    }

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
    }

    protected override void SafeAnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration)
        {
            return;
        }

        var members = classDeclaration.Members;

        for (int i = 0; i < members.Count - 1; i++)
        {
            var currentMember = members[i];
            var nextMember = members[i + 1];

            var currentKind = MemberKindHelper.GetMemberKind(currentMember);
            var nextKind = MemberKindHelper.GetMemberKind(nextMember);

            if (MemberSpacingHelper.RequiresBlankLineBetween(currentKind, nextKind))
            {
                var triviaInBetween = nextMember.GetLeadingTrivia();
                if (!HasRequiredBlankLine(triviaInBetween))
                {
                    var diagnostic = CreateDiagnostic(
                        nextMember.GetLocation(),
                        currentKind.ToString(),
                        nextKind.ToString());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }

    private static bool HasRequiredBlankLine(SyntaxTriviaList trivia)
    {
        int newlineCount = 0;
        foreach (var t in trivia)
        {
            if (t.IsKind(SyntaxKind.EndOfLineTrivia))
                newlineCount++;
            else if (!t.IsKind(SyntaxKind.WhitespaceTrivia))
                newlineCount = 0;
        }
        return newlineCount >= 2; // One blank line means two newlines
    }
}
