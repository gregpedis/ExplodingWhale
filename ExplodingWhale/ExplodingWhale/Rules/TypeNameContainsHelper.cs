using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace ExplodingWhale.Rules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class TypeNameContainsHelper : AnalyzerBase
{
    private const string target = "Helper";

    protected override string Id => "001";
    protected override string Title => "Type name contains 'Helper'";
    protected override string MessageFormat => "Type name '{0}' contains the word 'Helper'.";

    protected override void Register(AnalysisContext context) =>
        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);

    private void Analyze(SymbolAnalysisContext context)
    {
        var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
        var foundIndex = namedTypeSymbol.Name.IndexOf(target);
        if (foundIndex != -1)
        {
            var typeLocation = namedTypeSymbol.Locations[0];
            var diagnosticLocation = Location.Create(typeLocation.SourceTree, new TextSpan(typeLocation.SourceSpan.Start + foundIndex, target.Length));
            var diagnostic = Diagnostic.Create(Rule, diagnosticLocation, namedTypeSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
