using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace ExplodingWhale.Rules;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public partial class TypeMemberOrdering : AnalyzerBase
{
    protected override string Id => "008";
    protected override string Title => "Ordering of type members should be followed";
    protected override string MessageFormat => "Move this member above all {0}.";

    protected override void Register(AnalysisContext context) =>
        context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);

    private void Analyze(SymbolAnalysisContext context)
    {
        var type = (INamedTypeSymbol)context.Symbol;
        var memberPriorities = type.GetMembers().Where(FilterMember).Select(x => (Value: x, Priority: GetPriority(x))).ToArray();

        if (memberPriorities.Length <= 1)
        {
            return;
        }

        for (int i = 1; i < memberPriorities.Length; i++)
        {
            var current = memberPriorities[i];
            for (int j = 0; j < i; j++)
            {
                var previous = memberPriorities[j];
                if (current.Priority < previous.Priority)
                {
                    var diagnostic = Diagnostic.Create(Rule, current.Value.Locations[0], PriorityDescriptions[previous.Priority]);
                    context.ReportDiagnostic(diagnostic);
                    break;
                }
            }
        }
    }

    private static Priority GetPriority(ISymbol member) =>
        member switch
        {
            INamedTypeSymbol type => type.TypeKind == TypeKind.Enum ? Priority.NestedEnum : Priority.NestedType,
            IFieldSymbol field => field.IsConst ? Priority.Constant : Priority.Field,
            _ when member.IsAbstract => Priority.Abstract,
            _ when member.Kind == SymbolKind.Property => Priority.Property,
            IMethodSymbol method => method.MethodKind == MethodKind.Constructor ? Priority.Constructor : Priority.Method,
            _ => Priority.Unknown,
        };

    private static bool FilterMember(ISymbol symbol) =>
        !symbol.IsImplicitlyDeclared
        && (symbol is not IMethodSymbol method || ValidMethodKinds.Contains(method.MethodKind));
}

// Definitions go here for readability.
public partial class TypeMemberOrdering
{
    // we do not care about things like MethodKind.PropertyGet, etc.
    protected static readonly ImmutableHashSet<MethodKind> ValidMethodKinds = ImmutableHashSet.Create(
            MethodKind.Constructor,
            MethodKind.Destructor,
            MethodKind.ExplicitInterfaceImplementation,
            MethodKind.Ordinary,
            MethodKind.StaticConstructor);

    private enum Priority
    {
        Constant = 0,
        NestedEnum = 1,
        Field = 2,
        Abstract = 3,
        Property = 4,
        Constructor = 5,
        Method = 6,
        NestedType = 7,
        Unknown = 99, // defensive, in case this happens it will be considered maximum priority and be safely ignored.
    }

    // Priority.Constant is not needed here, since nothing needs to be moved above it (it has the lowest priority).
    private static readonly ImmutableDictionary<Priority, string> PriorityDescriptions = new Dictionary<Priority, string>
    {
        { Priority.NestedEnum, "nested enums" },
        { Priority.Field, "fields" },
        { Priority.Abstract, "abstract members" },
        { Priority.Property, "properties" },
        { Priority.Constructor, "constructors" },
        { Priority.Method, "methods" },
        { Priority.NestedType, "nested types" },
    }
    .ToImmutableDictionary();
}
