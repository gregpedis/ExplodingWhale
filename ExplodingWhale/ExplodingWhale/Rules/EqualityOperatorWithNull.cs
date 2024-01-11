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
    protected override string MessageFormat => "Replace '{0}' with '{1}' {2}when comparing with null.";

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
        var nullLeft = Null(context, node.Left);
        var nullRight = Null(context, node.Right);

        // we do not want to raise when both are null
        if (nullLeft && !nullRight)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), before, after, "and reverse the operands "));
        }
        else if (nullRight && !nullLeft)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), before, after, string.Empty));
        }
    }

    private static bool Null(SyntaxNodeAnalysisContext context, SyntaxNode node) =>
        context.SemanticModel.GetConstantValue(node) is { HasValue: true, Value: null };
}
