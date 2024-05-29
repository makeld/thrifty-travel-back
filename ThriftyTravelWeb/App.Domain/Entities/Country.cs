using Domain.Base;

namespace Domain.Entities;

public class Country : BaseEntityId
{
    public string Name { get; set; } = default!;
    public string Continent { get; set; } = default!;
}