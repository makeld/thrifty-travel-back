using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace WebApp.Models;

public abstract class BaseMetaDataId : BaseEntityId, IMetaData
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}