using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs;

public class UpdateTaskDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; }
}