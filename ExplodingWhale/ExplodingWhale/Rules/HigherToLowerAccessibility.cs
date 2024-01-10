using ExplodingWhale.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace ExplodingWhale.Rules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class HigherToLowerAccessibility : AnalyzerBase
{
    protected override string Id => "007";
    protected override string Title => "Members of the same kind should be ordered from higher to lower accessibility";
    protected override string MessageFormat => "Move member '{0}' above all members of the same kind with less accessibility.";

    protected static readonly ImmutableHashSet<MethodKind> ValidMethodKinds = ImmutableHashSet.Create(
                MethodKind.Constructor,
                MethodKind.Destructor,
                MethodKind.ExplicitInterfaceImplementation,
                MethodKind.Ordinary,
                MethodKind.StaticConstructor);

    protected override void Register(AnalysisContext context) =>
        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);

    private void Analyze(SymbolAnalysisContext context)
    {
        var type = (INamedTypeSymbol)context.Symbol;
        var groupedMembers = type.GetMembers().Where(FilterMember).GroupBy(GroupMember);
        foreach (var memberGroup in groupedMembers)
        {
            Analyze(context, memberGroup);
        }
    }

    private void Analyze(SymbolAnalysisContext context, IEnumerable<ISymbol> members)
    {
        var partitioned = members.PartitionBy(x => x.DeclaredAccessibility).Where(x => x.Key != Accessibility.NotApplicable);
        var currentAccessibility = partitioned.First().Key;
        foreach (var group in partitioned.Skip(1))
        {
            if (group.Key > currentAccessibility)
            {
                foreach (var member in group)
                {
                    var diagnostic = Diagnostic.Create(Rule, member.Locations[0], member.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else
            {
                // always check against the lowest accessibility found so far.
                // The goal is for every partition to never have something with lower accessibility above it.
                // For example, in the scenario we find public -> private -> protected, we know that protected has to be moved up.
                currentAccessibility = group.Key;
            }
        }
    }

    private static bool FilterMember(ISymbol symbol) =>
        !symbol.IsImplicitlyDeclared
        && (symbol is not IMethodSymbol method || ValidMethodKinds.Contains(method.MethodKind));

    private static (SymbolKind, MethodKind, bool) GroupMember(ISymbol symbol) => // both constructors and methods have symbol.Kind == Method
        symbol is IMethodSymbol method
            ? (method.Kind, method.MethodKind, method.IsStatic)
            : (symbol.Kind, default, symbol.IsStatic);
}
