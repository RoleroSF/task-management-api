using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskManagementApi.DTOs;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
    {
        var response = await _userService.GetAllUsersAsync();
        return Ok(response);
    }

    [HttpGet("by-id/{id:int}")]
    public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
    {
        var response = await _userService.GetUserByIdAsync(id);
        return Ok(response);
    }

    [HttpGet("by-email")]
    public async Task<ActionResult<UserResponseDto>> GetUserByEmail([FromQuery] string email)
    {
        var response = await _userService.GetUserByEmailAsync(email);
        return Ok(response);
    }

    [HttpPut("update/{id:int}")]
    public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, UpdateUserDto dto)
    {
        var response = await _userService.UpdateUserAsync(id, dto); 
        return Ok(response);
    }
}