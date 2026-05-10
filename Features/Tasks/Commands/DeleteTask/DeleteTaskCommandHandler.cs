using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Features.Tasks.Commands.DeleteTask;

public sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTaskCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        IQueryable<TaskItem> query = _context.Tasks;

        if (!_currentUserService.IsInRole("admin"))
        {
            var currentUserId = _currentUserService.GetUserId();
            query = query.Where(t => t.UserId == currentUserId);
        }
        var task = await query.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (task is null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}