using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;

namespace ExplodingWhale.Rules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MakeUppercase : AnalyzerBase
{
    protected override string Id => "001";
    protected override string Title => "Type name contains lowercase letters";
    protected override string MessageFormat => "Type name '{0}' contains lowercase letters";

    protected override void Register(AnalysisContext context)
    {
        var x = new MakeUppercase();
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md
        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);
    }

    private void Analyze(SymbolAnalysisContext context)
    {
        // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
        var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        // Find just those named type symbols with names containing lowercase letters.
        if (Array.Exists(namedTypeSymbol.Name.ToCharArray(), char.IsLower))
        {
            // For all such symbols, produce a diagnostic.
            var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}


