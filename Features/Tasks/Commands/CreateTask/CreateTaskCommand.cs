using MediatR;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Features.Tasks.Commands.CreateTask;

public sealed record CreateTaskCommand(CreateTaskDto CreateTaskDto) : IRequest<TaskResponseDto>;