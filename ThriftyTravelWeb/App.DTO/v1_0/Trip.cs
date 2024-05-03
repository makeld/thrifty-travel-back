using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Trip
{
    
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string ContestName { get; set; } = default!;

}