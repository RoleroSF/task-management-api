using MediatR;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Features.Tasks.Commands.CompleteAllTasks;

public sealed record CompleteAllTasksCommand : IRequest<MassUpdatedTasksCountDTO>;