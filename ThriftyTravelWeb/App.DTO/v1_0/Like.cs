namespace App.DTO.v1_0;

public class Like
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public Guid TripId { get; set; }
}