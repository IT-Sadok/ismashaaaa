namespace MakeupClone.Domain.Common;

public class UnpagedResult<T>
{
    public IEnumerable<T> Items { get; init; } =[];

    public int TotalCount { get; init; }

    public UnpagedResult() { }

    public UnpagedResult(IEnumerable<T> items, int totalCount)
    {
        Items = items ??[];
        TotalCount = totalCount;
    }
}