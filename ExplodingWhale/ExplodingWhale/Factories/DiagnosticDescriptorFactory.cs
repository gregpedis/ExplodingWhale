using Microsoft.CodeAnalysis;

namespace ExplodingWhale.Factories;

internal static class DiagnosticDescriptorFactory
{
    internal static DiagnosticDescriptor Create(
        string id,
        string title,
        string messageFormat,
        string ruleName,
        string category = "Styling",
        bool enabledByDefault = true)
        => new(
            id,
            title,
            messageFormat,
            category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: enabledByDefault,
            helpLinkUri: $"https://github.com/gregpedis/ExplodingWhale/tree/master/docs/reference/{id}_{ruleName}.md"
            );
}
