using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ExplodingWhale.Rules;

public abstract class InstanceAfterStaticMember: AnalyzerBase
{
    protected override string Title => $"Instance {MemberNamePlural} should be placed after static {MemberNamePlural}";
    protected override string MessageFormat => $$"""Move instance {{MemberName}} '{0}' after all the static {{MemberNamePlural}}.""";

    protected abstract string MemberName { get; }
    protected abstract string MemberNamePlural { get; }

    protected abstract IEnumerable<ISymbol> ApplicableMembers(INamedTypeSymbol container);

    protected override void Register(AnalysisContext context) =>
        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);

    private void Analyze(SymbolAnalysisContext context)
    {
        if (ApplicableMembers((INamedTypeSymbol)context.Symbol).Reverse().SkipWhile(x => !x.IsStatic).Where(x => !x.IsStatic) is { } badlyPlaced)
        {
            foreach (var member in badlyPlaced)
            {
                var diagnostic = Diagnostic.Create(Rule, member.Locations[0], member.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class InstanceAfterStaticFields : InstanceAfterStaticMember
{
    protected override string Id => "005";
    protected override string MemberName => "field";
    protected override string MemberNamePlural => "fields";

    protected override IEnumerable<ISymbol> ApplicableMembers(INamedTypeSymbol container) =>
        container.GetMembers().Where(x => x.Kind == SymbolKind.Field && ((IFieldSymbol)x).AssociatedSymbol is null); // Filter out auto-backing fields
}

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class InstanceAfterStaticProperties : InstanceAfterStaticMember
{
    protected override string Id => "006";
    protected override string MemberName => "property";
    protected override string MemberNamePlural => "properties";

    protected override IEnumerable<ISymbol> ApplicableMembers(INamedTypeSymbol container) =>
        container.GetMembers().Where(x => x.Kind == SymbolKind.Property);
}
