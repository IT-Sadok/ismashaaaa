namespace MakeupClone.Application.Helpers;

public static class PaginationHelper
{
    public static int CalculatePageNumber(int skip, int take)
    {
        return (skip / take) + 1;
    }
}