namespace TaskManagementApi.Helpers;

public static class UserHelper
{
    public static bool IsAllowedRoles(string role)
    {
        var allowedRoles = new[] { "admin", "user" };
        return allowedRoles.Contains(role.ToLower());
    }
}