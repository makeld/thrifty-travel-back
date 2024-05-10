using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Location
{
    public Guid Id { get; set; }
    public Guid CountryId { get; set; }
    
    [MaxLength(128)]
    public string LocationName { get; set; } = default!;
}