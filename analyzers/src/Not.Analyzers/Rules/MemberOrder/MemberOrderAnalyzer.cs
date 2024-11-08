using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Not.Analyzers.Base;
using Not.Analyzers.Members;

namespace Not.Analyzers.Rules.MemberOrder;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MemberOrderAnalyzer : AnalyzerBase
{
    public MemberOrderAnalyzer() : base(
        diagnosticId: "NA0003",
        title: "Member ordering rule violation",
        messageFormat: "Member {0} should follow {1}",
        description: "Members should be ordered accordingly.")
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
        if (context.Node is not TypeDeclarationSyntax classDeclaration)
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

            if (MemberOrderHelper.ComepareOrder(currentKind, nextKind) > 0)
            {
                var diagnostic = CreateDiagnostic(classDeclaration.GetLocation(), currentKind, nextKind);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
} 
