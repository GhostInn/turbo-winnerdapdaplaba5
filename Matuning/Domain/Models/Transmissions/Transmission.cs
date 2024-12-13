using Matuning.Domain.Models.Cars;

namespace Matuning.Domain.Models.Transmissions;

public class Transmission
{
    public int TransmissionId { get; set; }
    public string TransmissionTypeName { get; set; }

    // Навигационные свойства
    public ICollection<Car> Cars { get; set; }
}