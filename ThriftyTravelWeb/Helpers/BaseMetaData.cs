using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public abstract class BaseMetaData : IMetaData
{
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}