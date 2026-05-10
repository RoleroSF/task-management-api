using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Extensions;
using TaskManagementApi.Helpers;
using TaskManagementApi.Mappings;
using TaskManagementApi.Models;

namespace TaskManagementApi.Features.Tasks.Queries.GetTasks;

public sealed class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PagedResultDto<TaskResponseDto>>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTasksQueryHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResultDto<TaskResponseDto>> Handle(GetTasksQuery query, CancellationToken cancellationToken)
    {
        var page = PaginationHelper.NormalizePage(query.Parameters.Page);
        var pageSize = PaginationHelper.NormalizePageSize(query.Parameters.PageSize);

        IQueryable<TaskItem> tasksQuery = _context.Tasks.AsNoTracking();

        if (!_currentUserService.IsInRole("admin"))
        {
            var userId = _currentUserService.GetUserId();
            tasksQuery = tasksQuery.Where(t => t.UserId == userId);
        }

        tasksQuery = tasksQuery
            .ApplyFiltering(query.Parameters)
            .ApplySorting(query.Parameters);

        var totalCount = await tasksQuery.CountAsync(cancellationToken);

        var items = await tasksQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(TaskMapper.ToResponseDtoExpression)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<TaskResponseDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}