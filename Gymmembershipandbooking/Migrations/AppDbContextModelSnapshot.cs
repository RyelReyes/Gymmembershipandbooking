// Owner: Reyes | Type: class (entity -> table GymClasses)
using System.ComponentModel.DataAnnotations;

namespace GymApi.Models;

public class GymClass
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Description { get; set; }

    public int InstructorId { get; set; }
    public User? Instructor { get; set; }

    public DateTime StartTime { get; set; }

    public int DurationMinutes { get; set; }

    public int Capacity { get; set; }

    // Soft delete: a cancelled class is kept for history.
    public bool IsActive { get; set; } = true;

    public DateTime GetEndTime() => StartTime.AddMinutes(DurationMinutes);
}
