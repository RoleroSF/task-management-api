using TaskManagementApi.Models;

namespace TaskManagementApi.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}