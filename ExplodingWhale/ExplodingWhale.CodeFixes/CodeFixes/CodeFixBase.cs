using ExplodingWhale.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ExplodingWhale.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TAnalyzer)), Shared]
public abstract class CodeFixBase<TAnalyzer> : CodeFixProvider
    where TAnalyzer : AnalyzerBase, new()
{
    protected abstract string Title { get; }

    protected virtual Diagnostic Diagnostic(CodeFixContext context) => 
        context.Diagnostics.First();

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(new TAnalyzer().DiagnosticId);

    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
    public sealed override FixAllProvider GetFixAllProvider() =>
        WellKnownFixAllProviders.BatchFixer;


    protected void Register(CodeFixContext context, Diagnostic diagnostic, Func<CancellationToken, Task<Document>> changeDocument) =>
        context.RegisterCodeFix(CodeAction.Create(Title, changeDocument, diagnostic.Id), diagnostic);

    protected void Register(CodeFixContext context, Diagnostic diagnostic, Func<CancellationToken, Task<Solution>> changeSolution) =>
        context.RegisterCodeFix(CodeAction.Create(Title, changeSolution, diagnostic.Id), diagnostic);
}
