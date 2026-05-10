using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Mappings;
using TaskManagementApi.Models;

namespace TaskManagementApi.Features.Tasks.Queries.GetTaskById;

public sealed class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskResponseDto?>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public GetTaskByIdQueryHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<TaskResponseDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<TaskItem> taskQuery = _context.Tasks.AsNoTracking();

        if (!_currentUserService.IsInRole("admin"))
        {
            var currentUserId = _currentUserService.GetUserId();
            taskQuery = taskQuery.Where(t => t.UserId == currentUserId);
        }

        return await taskQuery
            .Select(TaskMapper.ToResponseDtoExpression)
            .FirstOrDefaultAsync(t => t.Id == request.Id ,cancellationToken);
    }
}