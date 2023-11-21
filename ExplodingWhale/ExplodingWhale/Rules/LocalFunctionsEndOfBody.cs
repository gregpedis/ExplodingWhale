using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ExplodingWhale.Rules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class LocalFunctionsEndOfBody : AnalyzerBase
{
    protected override string Id => "002";
    protected override string Title => "Local function is not at the end of the body";
    protected override string MessageFormat => "Move '{0}' at the end of the body.";

    protected override void Register(AnalysisContext context) =>
        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.LocalFunctionStatement);

    private void Analyze(SyntaxNodeAnalysisContext context)
    {
        var node = (LocalFunctionStatementSyntax)context.Node;
        if (BlockParent(node) is not { } block)
        {
            return;
        }

        var currentIndex = block.Statements.IndexOf(node);
        if (block.Statements.Skip(currentIndex + 1).Any(x => !x.IsKind(SyntaxKind.LocalFunctionStatement)))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, node.Identifier.GetLocation(), node.Identifier));
        }
    }

    private BlockSyntax BlockParent(SyntaxNode node)
    {
        while (node is not null && !node.IsKind(SyntaxKind.Block))
        {
            node = node.Parent;
        }
        return (BlockSyntax)node;
    }
}
