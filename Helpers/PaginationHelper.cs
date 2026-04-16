namespace TaskManagementApi.Helpers;

public static class PaginationHelper
{
    public static int NormalizePage(int page)
    {
        return page < 1 ? 1 : page;
    }

    public static int NormalizePageSize(int pageSize, int maxPageSize = 50)
    {
        if (pageSize < 1)
        {
            return 10;
        }

        return pageSize > maxPageSize ? maxPageSize : pageSize;
    }
}