using Matuning.Domain.Enums;
using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Models.Services;

public class Service
{
    public int ServiceId { get; set; }
    public int UserId { get; set; }
    public string ServiceName { get; set; }
    public string Description { get; set; }
    public OfferOrRequest OfferOrRequest { get; set; }
    public string Status { get; set; }
    public DateTime PostingDate { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public User User { get; set; }
}