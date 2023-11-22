using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ExplodingWhale.Rules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class StaticAfterInstanceMethods : AnalyzerBase
{
    protected override string Id => "004";
    protected override string Title => "Static methods should be placed after instance methods";
    protected override string MessageFormat => "Move static method '{0}' after all the instance methods";

    protected override void Register(AnalysisContext context) =>
        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);

    private void Analyze(SymbolAnalysisContext context)
    {
        var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        var methods = namedTypeSymbol.GetMembers()
            .Where(x => x.Kind == SymbolKind.Method)
            .Select(x => (IMethodSymbol)x)
            .Where(x => x.MethodKind == MethodKind.Ordinary);

        // Start looking at the methods in reverse.
        // Skip static methods until you hit a non-static one.
        // Try to find more static methods.
        // These are the badly placed ones.
        if (methods.Reverse().SkipWhile(x => x.IsStatic).Where(x => x.IsStatic) is { } badlyPlaced)
        {
            foreach (var staticMethod in badlyPlaced)
            {
                var diagnostic = Diagnostic.Create(Rule, staticMethod.Locations[0], staticMethod.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
