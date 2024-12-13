using Matuning.Domain.Models.Cars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matuning.Domain.Interfaces;

public interface ICarsRepository
{
    Task<Car> AddCarAsync(Car car);
    Task<Car> GetCarByIdAsync(int carId);
    Task<IEnumerable<Car>> GetCarsByUserIdAsync(int userId);
    Task<bool> UpdateCarAsync(Car car);
    Task<bool> DeleteCarAsync(int carId);
}