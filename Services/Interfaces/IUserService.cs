using TaskManagementApi.DTOs;

namespace TaskManagementApi.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> GetUserByIdAsync(int id);
    Task<UserResponseDto> GetUserByEmailAsync(string email);
    Task<IList<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto dto);
}    