using MediatR;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Features.Tasks.Commands.DeleteAllTasks;

public sealed record DeleteAllTasksCommand : IRequest<AffectedRowsDto>;