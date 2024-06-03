
namespace App.BLL.DTO;

public class AddTrip
{
    public string TripTitle { get; set; } = default!;

    public string? TripDescription { get; set; }

    public string? ImageUrl { get; set; } = default!;

    public string? ImageDescription { get; set; }
    
    public string? TripCategory { get; set; }
    
    public Guid? TripId { get; set; }
    public Guid? TripUserId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? TripCategoryId { get; set; }
    public Guid? ImageId { get; set; }
}