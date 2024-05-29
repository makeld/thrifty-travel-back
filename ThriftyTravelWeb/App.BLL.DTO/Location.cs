using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Location: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid CountryId { get; set; }

    public string LocationName { get; set; } = default!;
}