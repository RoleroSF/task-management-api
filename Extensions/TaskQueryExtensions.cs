using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Extensions;

public static class TaskQueryExtensions
{
    public static IQueryable<TaskItem> ApplyFiltering(this IQueryable<TaskItem> query, TaskQueryParametersDto queryParams)
    {
        if (queryParams.IsCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == queryParams.IsCompleted.Value);
        }

        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.Trim().ToLower();

            query = query.Where(t =>
                t.Title.ToLower().Contains(search) ||
                (t.Description != null && t.Description.ToLower().Contains(search)));
        }

        return query;
    }

    public static IQueryable<TaskItem> ApplySorting(this IQueryable<TaskItem> query,TaskQueryParametersDto queryParams)
    {
        var sortBy = queryParams.SortBy?.Trim().ToLower();
        var isDesc = queryParams.SortOrder?.Trim().ToLower() == "desc";

        return sortBy switch
        {
            "title" => isDesc
                ? query.OrderByDescending(t => t.Title)
                : query.OrderBy(t => t.Title),

            "createdat" => isDesc
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt),

            "iscompleted" => isDesc
                ? query.OrderByDescending(t => t.IsCompleted)
                : query.OrderBy(t => t.IsCompleted),

            _ => isDesc
                ? query.OrderByDescending(t => t.Id)
                : query.OrderBy(t => t.Id)
        };
    }
}