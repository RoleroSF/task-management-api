using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Extensions;
using TaskManagementApi.Helpers;
using TaskManagementApi.Mappings;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public TasksController(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<TaskResponseDto>>> GetAll([FromQuery] TaskQueryParametersDto queryParams)
    {
        var page = PaginationHelper.NormalizePage(queryParams.Page);
        var pageSize = PaginationHelper.NormalizePageSize(queryParams.PageSize);

        IQueryable<TaskItem> query = _context.Tasks.AsNoTracking();

        if (!_currentUserService.IsInRole("admin"))
        {
            var currentUserId = _currentUserService.GetUserId();
            query = query.Where(t => t.UserId == currentUserId);
        }

        query = query.ApplyFiltering(queryParams).ApplySorting(queryParams);

        var totalCount = await query.CountAsync();

        var tasks = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(TaskMapper.ToResponseDtoExpression)
            .ToListAsync();

        var result = new PagedResultDto<TaskResponseDto>
        {
            Items = tasks,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        IQueryable<TaskItem> query = _context.Tasks.AsNoTracking();

        if (!_currentUserService.IsInRole("admin"))
        {
            var currentUserId = _currentUserService.GetUserId();
            query = query.Where(t => t.UserId == currentUserId);
        }

        var task = await query
            .Where(t => t.Id == id)
            .Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (task is null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
    {
        var currentUserId = _currentUserService.GetUserId();

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UserId = currentUserId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var response = TaskMapper.ToResponseDto(task);

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> Update(int id, UpdateTaskDto dto)
    {
        IQueryable<TaskItem> query = _context.Tasks.AsNoTracking();

        if (!_currentUserService.IsInRole("admin"))
        {
            var currentUserId = _currentUserService.GetUserId();
            query = query.Where(t => t.UserId == currentUserId);
        }

        var task = await query.FirstOrDefaultAsync(t => t.Id == id);

        if (task is null) return NotFound();

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;

        await _context.SaveChangesAsync();

        var response = TaskMapper.ToResponseDto(task);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        IQueryable<TaskItem> query = _context.Tasks.AsNoTracking();

        if (!_currentUserService.IsInRole("admin"))
        {
            var currentUserId = _currentUserService.GetUserId();
            query = query.Where(t => t.UserId == currentUserId);
        }
        var task = await query.FirstOrDefaultAsync(t => t.Id == id);

        if (task is null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Test exception");
    }
}