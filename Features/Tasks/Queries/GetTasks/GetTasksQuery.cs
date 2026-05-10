using MediatR;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Features.Tasks.Queries.GetTasks;

public sealed record GetTasksQuery(TaskQueryParametersDto Parameters) : IRequest<PagedResultDto<TaskResponseDto>>;