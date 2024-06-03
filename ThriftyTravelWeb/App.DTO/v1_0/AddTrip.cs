using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class AddTrip
{
    [MaxLength(128)]
    public string TripTitle { get; set; } = default!;
    
    [MaxLength(512)]
    public string? TripDescription { get; set; }
    
    [MaxLength(256)]
    public string? ImageUrl { get; set; } = default!;
    
    [MaxLength(128)]
    public string? ImageDescription { get; set; }
    
    [MaxLength(128)]
    public string? TripCategory { get; set; }
    
    public Guid? TripId { get; set; }
    public Guid? TripUserId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? TripCategoryId { get; set; }
    public Guid? ImageId { get; set; }
}