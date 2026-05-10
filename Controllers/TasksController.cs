using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskManagementApi.DTOs;
using TaskManagementApi.Features.Tasks.Commands.CreateTask;
using TaskManagementApi.Features.Tasks.Commands.DeleteTask;
using TaskManagementApi.Features.Tasks.Commands.UpdateTask;
using TaskManagementApi.Features.Tasks.Queries.GetTaskById;
using TaskManagementApi.Features.Tasks.Queries.GetTasks;

namespace TaskManagementApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ISender _sender;
    public TasksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<TaskResponseDto>>> GetAll([FromQuery] TaskQueryParametersDto queryParams, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTasksQuery(queryParams), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTaskByIdQuery(id), cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateTaskCommand(dto), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> Update(int id, UpdateTaskDto dto)
    {
        var response = await _sender.Send(new UpdateTaskCommand(id, dto), CancellationToken.None);

        if (response is null) return NotFound();

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteTaskCommand(id), cancellationToken);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Test exception");
    }
}