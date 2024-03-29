﻿using ExplodingWhale.Factories;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace ExplodingWhale.Rules;

public abstract class AnalyzerBase : DiagnosticAnalyzer
{
    public string DiagnosticId => $"{Definitions.RULE_PREFIX}{Id}";
    public DiagnosticDescriptor Rule => DiagnosticDescriptorFactory.Create(DiagnosticId, Title, MessageFormat, GetType().Name);
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    protected abstract string Id { get; }
    protected abstract string Title { get; }
    protected abstract string MessageFormat { get; }

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        Register(context);
    }

    protected abstract void Register(AnalysisContext context);
}
