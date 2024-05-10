namespace App.DTO.v1_0;

public class TripLocation
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Guid LocationId { get; set; }
}