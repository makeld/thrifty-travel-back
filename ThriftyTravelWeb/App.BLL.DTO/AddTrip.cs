
namespace App.BLL.DTO;

public class AddTrip
{
    public string TripTitle { get; set; } = default!;

    public string? TripDescription { get; set; }

    public string? ImageUrl { get; set; } = default!;

    public string? ImageDescription { get; set; }
    
    public string? TripCategory { get; set; }
}