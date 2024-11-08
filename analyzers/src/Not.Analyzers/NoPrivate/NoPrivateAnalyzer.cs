using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Not.Analyzers.Base;

namespace Not.Analyzers.NoPrivate
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NoPrivateAnalyzer : AnalyzerBase
    {
        public const string DiagnosticId = "NA0001";
        private static readonly LocalizableString Title = "Avoid using 'private' keyword";
        private static readonly LocalizableString MessageFormat = "The 'private' keyword is not necessary as members are private by default";
        private static readonly LocalizableString Description = "Avoid using 'private' keyword.";
        private const string Category = "CodeStyle";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        protected override IEnumerable<DiagnosticDescriptor> CustomRules => [Rule];

        protected override void SafeAnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not MemberDeclarationSyntax declaration) 
            {
                return;
            }

            var privateModifier = declaration.Modifiers.FirstOrDefault(m => m.IsKind(SyntaxKind.PrivateKeyword));
            if (privateModifier != default)
            {
                var diagnostic = Diagnostic.Create(Rule, privateModifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, 
                SyntaxKind.FieldDeclaration, 
                SyntaxKind.MethodDeclaration, 
                SyntaxKind.PropertyDeclaration,
                SyntaxKind.ConstructorDeclaration,
                SyntaxKind.ClassDeclaration);
        }
    }
}
