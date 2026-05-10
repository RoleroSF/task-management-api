using MediatR;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Features.Tasks.Queries.GetTaskById;

public sealed record GetTaskByIdQuery(int Id) : IRequest<TaskResponseDto?>;