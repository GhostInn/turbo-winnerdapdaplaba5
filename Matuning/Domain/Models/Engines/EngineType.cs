using Matuning.Domain.Models.Cars;

namespace Matuning.Domain.Models.Engines;

public class EngineType
{
    public int EngineTypeId { get; set; }
    public string EngineTypeName { get; set; }

    // Навигационные свойства
    public ICollection<Car> Cars { get; set; }
}