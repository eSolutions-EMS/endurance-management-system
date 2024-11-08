using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Not.Analyzers.NoPrivate;

namespace NoPrivate.Tests
{
    public class NoPrivateAnalyzerTests
    {
        [Theory]
        [InlineData(
            @"private MyClass() { }",
            @"MyClass() { }")]
        [InlineData(
            @"private void MyMethod() { }",
            @"void MyMethod() { }")]
        [InlineData(
            @"private int MyProperty { get; set; }",
            @"int MyProperty { get; set; }")]
        [InlineData(
            @"private class MyPrivateClass { }",
            @"class MyPrivateClass { }")]
        [InlineData(
            @"private bool _field;",
            @"bool _field;")]
        [InlineData(
            @"private readonly static bool _readonlyStaticField;",
            @"readonly static bool _readonlyStaticField;")]
        [InlineData(
            @"private const bool _readonlyStaticField = true;",
            @"const bool _readonlyStaticField = true;")]
        public async Task TestPrivateClass(string test, string expected)
        {
            var testClass = $@"class MyClass {{ {test} }}";
            var expectedClass = $@"class MyClass {{ {expected} }}";
            var context = new CSharpCodeFixTest<NoPrivateAnalyzer, NoPrivateFixProvider, DefaultVerifier>
            {
                ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
                TestCode = testClass,
                FixedCode = expectedClass,
                ExpectedDiagnostics =
                {
                    // Specify the diagnostic with message
                    DiagnosticResult.CompilerWarning("NA0001")
                        .WithMessage("The 'private' keyword is not necessary as members are private by default")
                        .WithSpan(1, 17, 1, 24)
                }
            };

            await context.RunAsync();
        }
    }
}
