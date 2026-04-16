using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    [MinLength(3)]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}