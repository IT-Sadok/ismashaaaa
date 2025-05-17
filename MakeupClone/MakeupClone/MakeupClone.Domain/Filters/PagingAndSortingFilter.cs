namespace MakeupClone.Domain.Filters;

public class PagingAndSortingFilter
{
    public int Take { get; set; } = 10;

    public int Skip { get; set; } = 0;

    public string? OrderBy { get; set; }

    public bool Descending { get; set; } = false;
}
