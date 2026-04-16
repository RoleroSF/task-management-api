using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class UpdateUserDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MinLength(3)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [MinLength(3)]
        public string Role { get; set; } = string.Empty;

        [MinLength(3)]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
    }
}
