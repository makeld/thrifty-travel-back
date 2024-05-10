using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Country: IDomainEntityId
{
    public Guid Id { get; set; }

    public enum Name;
    public enum Continent;
}