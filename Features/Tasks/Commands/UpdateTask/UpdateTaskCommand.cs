using MediatR;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Features.Tasks.Commands.UpdateTask;

public sealed record UpdateTaskCommand(int Id, UpdateTaskDto UpdateTaskDto) : IRequest<TaskResponseDto>;