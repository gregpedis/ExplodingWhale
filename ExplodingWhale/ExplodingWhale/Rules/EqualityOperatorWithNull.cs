using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ExplodingWhale.Rules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EqualityOperatorWithNull : AnalyzerBase
{
    protected override string Id => "003";
    protected override string Title => "Use 'is (not)' to compare with null";
    protected override string MessageFormat => "Replace '{0}' with '{1}' when comparing with null.";

    protected override void Register(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(
            c => Analyze(c, "!=", "is not"), SyntaxKind.NotEqualsExpression);

        context.RegisterSyntaxNodeAction(
            c => Analyze(c, "==", "is"), SyntaxKind.EqualsExpression);
    }

    private void Analyze(SyntaxNodeAnalysisContext context, string before, string after)
    {
        var node = (BinaryExpressionSyntax)context.Node;
        if (Null(context, node.Left) ^ Null(context, node.Right)) // XOR because "null is (not) null" is not valid 
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), before, after));
        }
    }

    private static bool Null(SyntaxNodeAnalysisContext context, SyntaxNode node) =>
        context.SemanticModel.GetConstantValue(node) is { HasValue: true, Value: null };
}
