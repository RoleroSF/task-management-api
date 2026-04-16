using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs;

public class CreateTaskDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}