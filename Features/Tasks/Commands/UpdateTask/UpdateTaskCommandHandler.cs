using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Mappings;
using TaskManagementApi.Models;

namespace TaskManagementApi.Features.Tasks.Commands.UpdateTask;

public sealed class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskResponseDto?>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTaskCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<TaskResponseDto?> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        IQueryable<TaskItem> query = _context.Tasks;

        if (!_currentUserService.IsInRole("admin"))
        {
            var userId = _currentUserService.GetUserId();
            query = query.Where(t => t.UserId == userId);
        }

        var task = await query.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (task is null)
            return null;

        task.Title = command.UpdateTaskDto.Title;
        task.Description = command.UpdateTaskDto.Description;
        task.IsCompleted = command.UpdateTaskDto.IsCompleted;

        await _context.SaveChangesAsync(cancellationToken);

        return TaskMapper.ToResponseDto(task);
    }
}