using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Trip
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string Title { get; set; } = default!;
    
    [MaxLength(512)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}