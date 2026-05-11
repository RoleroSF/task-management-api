using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Features.Tasks.Commands.CompleteAllTasks;

public sealed class CompleteAllTasksCommandHandler : IRequestHandler<CompleteAllTasksCommand, AffectedRowsDto>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public CompleteAllTasksCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<AffectedRowsDto> Handle(CompleteAllTasksCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.GetUserId();

        var updatedCount = await _context.Tasks
                   .Where(x => x.UserId == currentUserId && x.IsCompleted == false)
                   .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsCompleted, true), cancellationToken);

        return new AffectedRowsDto { Count = updatedCount };
    }
}