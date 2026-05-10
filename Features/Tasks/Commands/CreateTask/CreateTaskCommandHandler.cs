using MediatR;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Mappings;
using TaskManagementApi.Models;

namespace TaskManagementApi.Features.Tasks.Commands.CreateTask;

public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponseDto>
{
    private readonly AppDbContext _context; 
    private readonly ICurrentUserService _currentUserService;
    public CreateTaskCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<TaskResponseDto> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.GetUserId();

        var task = new TaskItem
        {
            Title = command.CreateTaskDto.Title,
            Description = command.CreateTaskDto.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UserId = currentUserId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(cancellationToken);

        return TaskMapper.ToResponseDto(task);
    }
}