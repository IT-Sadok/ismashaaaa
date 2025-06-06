using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Helpers;

public static class FilterBuilder
{
    public static PagingAndSortingFilter Build(int pageNumber, int pageSize, string? sortBy, bool descending)
    {
        return new PagingAndSortingFilter
        {
            Skip = (pageNumber - 1) * pageSize,
            Take = pageSize,
            OrderBy = sortBy,
            Descending = descending,
        };
    }
}