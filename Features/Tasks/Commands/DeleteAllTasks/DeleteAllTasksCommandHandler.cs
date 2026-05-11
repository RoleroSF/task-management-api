using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.DTOs;
using TaskManagementApi.Data;

namespace TaskManagementApi.Features.Tasks.Commands.DeleteAllTasks;

public class DeleteAllTasksCommandHandler : IRequestHandler<DeleteAllTasksCommand, MassUpdatedTasksCountDTO>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    public DeleteAllTasksCommandHandler(AppDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }
    public async Task<MassUpdatedTasksCountDTO> Handle(DeleteAllTasksCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.GetUserId();
       
        var result = await _dbContext.Tasks
            .Where(x => x.UserId == currentUserId)
            .ExecuteDeleteAsync(cancellationToken);

        return new MassUpdatedTasksCountDTO { Count = result };
    }
}