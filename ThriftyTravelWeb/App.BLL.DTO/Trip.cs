using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Trip: IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    // [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(ContestName))]
    public string TripName { get; set; } = default!;
}
