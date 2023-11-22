using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ExplodingWhale.TestingFramework;

public static class AnalyzerVerifier
{
    private const string OFFSET_GROUP = @$"(@(?<{nameof(OFFSET_GROUP)}>(\+|-)\d+))?";
    private const string MESSAGE_GROUP = @$"( {{(?<{nameof(MESSAGE_GROUP)}>.*)}})?";
    private static readonly Regex EXPECTED_REGEX = new($"// Bad{OFFSET_GROUP}{MESSAGE_GROUP}", RegexOptions.Compiled);
    private static readonly Regex LOCATION_REGEX = new(@$"\s*//\s*(?<{nameof(LOCATION_REGEX)}>\^+)\s*?", RegexOptions.Compiled);

    public static void Verify<TAnalyzer>(string code, OutputKind projectKind = OutputKind.DynamicallyLinkedLibrary)
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        var tree = SyntaxFactory.ParseSyntaxTree(code, new CSharpParseOptions(LanguageVersion.CSharp11));

        var diagnostics = CSharpCompilation.Create("InMemoryCompilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .WithOptions(new(projectKind))
            .AddSyntaxTrees(tree)
            .WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>().Add(new TAnalyzer()))
            .GetAllDiagnosticsAsync()
            .Result;

        var actual = Actual(diagnostics);
        var expected = Expected(code);

        PrettyPrint(actual);
        expected.Should().BeEquivalentTo(actual);
    }

    private static void PrettyPrint(IssueLocation[] issues)
    {
        for (int i = 0; i < issues.Length; i++)
        {
            var issue = issues[i];
            Debug.WriteLine($"Issue #{i}: Span L{issue.Line}:{issue.ColFrom}-{issue.ColTo}");
            Debug.WriteLine(issue.Message);
        }
    }

    private static IssueLocation[] Actual(ImmutableArray<Diagnostic> diagnostics) =>
        diagnostics.Where(d => d.Id.StartsWith(Constants.RULE_PREFIX)).Select(IssueLocation.Create).OrderBy(x => (x.Line, x.ColFrom, x.ColTo)).ToArray();

    // Support:
    // // Bad  
    // // Bad@+42
    // // Bad@-42
    // // Bad {message}
    // // Bad@+42 {message}
    // // ^^^^^
    private static IssueLocation[] Expected(string code)
    {
        var lines = code.Split(Environment.NewLine);
        var expected = new List<IssueLocation>();

        for (int lineIdx = 0; lineIdx < lines.Length; lineIdx++)
        {
            if (EXPECTED_REGEX.Match(lines[lineIdx]) is { Success: true } match)
            {
                string message = match.Groups[nameof(MESSAGE_GROUP)] is { Success: true } messageGroup ? messageGroup.Value.Trim() : null;
                int offset = match.Groups[nameof(OFFSET_GROUP)] is { Success: true } offsetGroup ? int.Parse(offsetGroup.Value) : 0;
                var realNumberIdx = lineIdx + offset;
                var curr = new IssueLocation(realNumberIdx, message);

                if (LOCATION_REGEX.Match(lines[realNumberIdx + 1]) is { Success: true } locationMatch
                    && locationMatch.Groups[nameof(LOCATION_REGEX)] is { Success: true } locationGroup)
                {
                    curr.ColFrom = locationGroup.Index;
                    curr.ColTo = locationGroup.Index + locationGroup.Length;
                }
                expected.Add(curr);
            }
        }
        return expected.OrderBy(x => (x.Line, x.ColFrom, x.ColTo)).ToArray();
    }
}
