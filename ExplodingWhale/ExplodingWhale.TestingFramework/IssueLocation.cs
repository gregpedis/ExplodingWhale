namespace ExplodingWhale.TestingFramework;

public sealed class IssueLocation : IEquatable<IssueLocation>
{
    public int Line { get; set; }
    public int? ColFrom { get; set; }
    public int? ColTo { get; set; }
    public string Message { get; set; }

    public IssueLocation(int line, string message = null) 
        : this(line, null, null, message)
    { }

    private IssueLocation(int lineFrom, int? colFrom, int? colTo, string message)
    {
        Line = lineFrom;
        ColFrom = colFrom;
        ColTo = colTo;
        Message = message;
    }

    public override bool Equals(object obj) 
        => Equals(obj as IssueLocation);

    public bool Equals(IssueLocation other)
    {
        return other is not null
            && KindOfEqual(Line, other?.Line)
            && KindOfEqual(ColFrom, other?.ColFrom)
            && KindOfEqual(ColTo, other?.ColTo)
            && KindOfEqual(Message, other?.Message);

        static bool KindOfEqual<T>(T left, T right) => 
            left is null || right is null || left.Equals(right);
    }

    public override int GetHashCode() =>
        (Line, ColFrom, ColTo, Message).GetHashCode();

    public static IssueLocation Create(Diagnostic diagnostic)
    {
        var span = diagnostic.Location.GetLineSpan();
        return new(
            span.StartLinePosition.Line,
            span.StartLinePosition.Character,
            span.EndLinePosition.Character,
            diagnostic.ToString().Split(":")[2].Trim());
    }
}
