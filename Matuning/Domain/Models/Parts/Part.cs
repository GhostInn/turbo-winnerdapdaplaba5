using Matuning.Domain.Models.Cars;
using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Models.Parts;

public class Part
{
    public int PartId { get; set; }
    public int UserId { get; set; }
    public int CarId { get; set; }
    public string PartName { get; set; }
    public string Description { get; set; }
    public string Condition { get; set; }
    public int AvailabilityStatusId { get; set; }
    public DateTime PostingDate { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public User User { get; set; }
    public Car Car { get; set; }
    public PartStatus AvailabilityStatus { get; set; }
}