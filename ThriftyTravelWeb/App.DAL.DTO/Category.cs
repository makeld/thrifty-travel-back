using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Category: IDomainEntityId
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
}