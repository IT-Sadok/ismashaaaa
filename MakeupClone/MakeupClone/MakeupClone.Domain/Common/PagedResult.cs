﻿namespace MakeupClone.Domain.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; init; } =[];

    public int TotalCount { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasNextPage => PageNumber < TotalPages;

    public bool HasPreviousPage => PageNumber > 1;

    public PagedResult() { }

    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items ??[];
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}