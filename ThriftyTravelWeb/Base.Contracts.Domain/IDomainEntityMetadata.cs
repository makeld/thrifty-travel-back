namespace Base.Contracts.Domain;

public interface IDomainEntityMetadata
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}