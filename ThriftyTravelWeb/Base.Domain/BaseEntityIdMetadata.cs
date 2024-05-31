using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace Domain.Base;

public abstract class BaseEntityIdMetadata : BaseEntityIdMetadata<Guid>, IDomainEntityId
{
    
}

public abstract class BaseEntityIdMetadata<TKey> : BaseEntityId<TKey>, IDomainEntityMetadata
    where TKey : IEquatable<TKey>
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}