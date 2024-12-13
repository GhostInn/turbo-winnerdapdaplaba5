namespace Matuning.Domain.Models.Parts;

public class PartStatus
{
    public int StatusId { get; set; }
    public string StatusName { get; set; }

    // Навигационные свойства
    public ICollection<Part> Parts { get; set; }
}