using Matuning.Domain.Models.Engines;
using Matuning.Domain.Models.Parts;
using Matuning.Domain.Models.Transmissions;
using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Models.Cars;

public class Car
{
    public int CarId { get; set; }
    public int UserId { get; set; }
    public string Make { get; set; } // Марка
    public string Model { get; set; } // Модель
    public short Year { get; set; } // Год выпуска
    public int EngineTypeId { get; set; }
    public int TransmissionId { get; set; }

    // Навигационные свойства
    public User User { get; set; }
    public EngineType EngineType { get; set; }
    public Transmission Transmission { get; set; }
    public ICollection<Part> Parts { get; set; }
}