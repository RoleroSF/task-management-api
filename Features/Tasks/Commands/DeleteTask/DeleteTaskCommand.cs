using MediatR;

namespace TaskManagementApi.Features.Tasks.Commands.DeleteTask;

public sealed record DeleteTaskCommand(int Id) : IRequest<bool>;