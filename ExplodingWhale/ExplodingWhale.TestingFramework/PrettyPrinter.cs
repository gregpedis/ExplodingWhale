using System.Diagnostics;

namespace ExplodingWhale.TestingFramework;

internal static class PrettyPrinter
{
    private const int PADDING = 8;

    public static void Print(IssueLocation[] actual, IssueLocation[] expected)
    {
        var smaller = actual.Length < expected.Length ? actual : expected;

        for (int i = 0; i < smaller.Length; i++)
        {
            var a = actual[i];
            var e = expected[i];
            Print(a, i, "Actual");
            Print(e, i, "Expected");
            PrintSeparator();
        }

        if (smaller == actual)
        {
            for (int i = actual.Length; i < expected.Length; i++)
            {
                PrintNotFound(i, "Actual");
                Print(expected[i], i, "Expected");
                PrintSeparator();
            }
        }
        else
        {
            for (int i = expected.Length; i < actual.Length; i++)
            {
                Print(actual[i], i, "Actual");
                PrintNotFound(i, "Expected");
                PrintSeparator();
            }
        }
    }

    private static void Print(IssueLocation issue, int index, string type) =>
        Debug.WriteLine($"{type.PadRight(PADDING)} #{index}: L{issue.Line}:{issue.ColFrom}-{issue.ColTo} {{ {issue.Message} }}");

    private static void PrintNotFound(int index, string type) =>
        Debug.WriteLine($"{type.PadRight(PADDING)} #{index}: <NOT FOUND>");

    private static void PrintSeparator() =>
        Debug.WriteLine("=================");
}
