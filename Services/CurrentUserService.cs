using System.Security.Claims;

namespace TaskManagementApi.Services;

public class CurrentUserService : ICurrentUserService
{
    public readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userIdClaims = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(string.IsNullOrWhiteSpace(userIdClaims) || !int.TryParse(userIdClaims, out var userId))
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");

        return userId;
    }

    public bool IsInRole(string role)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return user?.IsInRole(role) ?? false;
    }
}