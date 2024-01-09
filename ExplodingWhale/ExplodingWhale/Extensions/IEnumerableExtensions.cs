using System;

namespace ExplodingWhale.Extensions;

public static class IEnumerableExtensions
{
    public class PartitionChunk<TKey, TValue> : IEnumerable<TValue>
    {
        public TKey Key { get; }
        public List<TValue> Values { get; }

        public PartitionChunk(TKey key)
        {
            Key = key;
            Values = new();
        }

        public void Add(TValue item) =>
            Values.Add(item);

        public IEnumerator<TValue> GetEnumerator() =>
            Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            Values.GetEnumerator();
    }

    /// <summary>
    /// Splits a list on sub-lists based on a lambda that returns a TKey for every value.
    /// This is different to GroupBy, since it splits *every time* the lambda returns a different value.
    /// </summary>
    /// <example>
    /// For example, if called on "aaba" with the id lambda (x => x):
    /// GroupBy: [(aaa), (b)]
    /// PartitionBy: [(aa), (b), (a)]
    /// </example>
    public static IEnumerable<PartitionChunk<TKey, TValue>> PartitionBy<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> partitionSelector)
    {
        if (!source.Any())
        {
            yield break;
        }

        var first = source.First();
        var check = partitionSelector(first);
        var chunk = new PartitionChunk<TKey, TValue>(check) { first };

        foreach (var item in source.Skip(1))
        {
            if (check.Equals(partitionSelector(item)))
            {
                chunk.Add(item);
            }
            else
            {
                yield return chunk;
                check = partitionSelector(item);
                chunk = new PartitionChunk<TKey, TValue>(check) { item };
            }
        }

        yield return chunk; // there will always be at least one item in chunk
    }
}
