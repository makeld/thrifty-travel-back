using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Trip: IDomainEntityId
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
