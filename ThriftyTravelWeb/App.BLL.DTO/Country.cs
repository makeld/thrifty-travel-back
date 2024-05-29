using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Country: IDomainEntityId
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string Continent { get; set; } = default!;

}