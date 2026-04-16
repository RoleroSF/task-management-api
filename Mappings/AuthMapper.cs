using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Mappings;

public static class AuthMapper
{
    public static AuthResponseDto ToResponseDto(User user, string token)
    {
        return new AuthResponseDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            Token = token
        };
    }
}