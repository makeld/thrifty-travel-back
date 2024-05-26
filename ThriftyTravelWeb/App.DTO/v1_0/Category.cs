using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Category
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public ICollection<TripCategory>? TripCategories { get; set; }

}