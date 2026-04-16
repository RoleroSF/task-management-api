namespace TaskManagementApi.DTOs;

public class TaskQueryParametersDto
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public bool? IsCompleted { get; set; }

    public string? Search { get; set; }

    public string? SortBy { get; set; } = "id";

    public string? SortOrder { get; set; } = "asc";
}