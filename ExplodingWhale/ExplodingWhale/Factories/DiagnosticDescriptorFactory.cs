using Microsoft.CodeAnalysis;

namespace ExplodingWhale.Factories;

internal static class DiagnosticDescriptorFactory
{
    internal static DiagnosticDescriptor Create(
        string id,
        string title,
        string messageFormat,
        string category = null,
        bool enabledByDefault = true)
        => new(
            id,
            title,
            messageFormat,
            category ?? Constants.DEFAULT_CATEGORY,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: enabledByDefault);
}
