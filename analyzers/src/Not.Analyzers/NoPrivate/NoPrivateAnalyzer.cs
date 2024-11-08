using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Not.Analyzers.NoPrivate
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NoPrivateAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "NA0001";
        private static readonly LocalizableString Title = "Avoid using 'private' keyword";
        private static readonly LocalizableString MessageFormat = "The 'private' keyword is not necessary as members are private by default";
        private static readonly LocalizableString Description = "Avoid using 'private' keyword.";
        private const string Category = "CodeStyle";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        private static readonly DiagnosticDescriptor ErrorRule = new DiagnosticDescriptor(
            "NA0002",
            "Internal analyzer error",
            "An error occurred in NoPrivateAnalyzer: {0}",
            "Debug",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
            ImmutableArray.Create(Rule, ErrorRule);

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

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            try
            {
                var declaration = context.Node as MemberDeclarationSyntax;
                if (declaration == null) return;

                var privateModifier = declaration.Modifiers
                    .FirstOrDefault(m => m.IsKind(SyntaxKind.PrivateKeyword));
                
                if (privateModifier != default)
                {
                    var diagnostic = Diagnostic.Create(Rule, privateModifier.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
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
    }
}
