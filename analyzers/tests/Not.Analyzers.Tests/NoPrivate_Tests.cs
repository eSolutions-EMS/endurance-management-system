using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Not.Analyzers.NoPrivate;

namespace NoPrivate.Tests
{
    public class NoPrivateAnalyzerTests
    {
        [Theory]
        [InlineData(
            @"class MyClass { private MyClass() { } }",
            @"class MyClass { MyClass() { } }",
            1, 17, 1, 38)]
        [InlineData(
            @"class MyClass{ private void MyMethod() { } }",
            @"class MyClass{ void MyMethod() { } }",
            1, 16, 1, 43)]
        [InlineData(
            @"class MyClass { private int MyProperty { get; set; } }",
            @"class MyClass { int MyProperty { get; set; } }",
            1, 17, 1, 53)]
        [InlineData(
            @"class MyClass { private bool _field; }",
            @"class MyClass { bool _field; }",
            1, 17, 1, 37)]
        [InlineData(
            @"class MyClass { private class MyPrivateClass { } }",
            @"class MyClass { class MyPrivateClass { } }",
            1, 17, 1, 49)]
        public async Task TestPrivateClass(string test, string expected, int startLine, int startColumn, int endLine, int endColumn)
        {
            var context = new CSharpCodeFixTest<NoPrivateAnalyzer, NoPrivateFixProvider, DefaultVerifier>
            {
                ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
                TestCode = test,
                FixedCode = expected,
                ExpectedDiagnostics =
                {
                    // Specify the diagnostic with message
                    DiagnosticResult.CompilerWarning("NA0001")
                        .WithMessage("The 'private' keyword is not necessary as members are private by default")
                        .WithSpan(startLine, startColumn, endLine, endColumn)
                }
            };

            await context.RunAsync();
        }
    }
}
