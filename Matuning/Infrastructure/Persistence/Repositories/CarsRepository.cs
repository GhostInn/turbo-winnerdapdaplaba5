using Matuning.Domain.Interfaces;
using Matuning.Domain.Models.Cars;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matuning.Infrastructure;

public class CarsRepository : ICarsRepository
{
    private readonly ApplicationDbContext _context;

    public CarsRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Car> AddCarAsync(Car car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task<Car> GetCarByIdAsync(int carId)
    {
        return await _context.Cars.Include(c => c.User).FirstOrDefaultAsync(c => c.CarId == carId);
    }

    public async Task<IEnumerable<Car>> GetCarsByUserIdAsync(int userId)
    {
        return await _context.Cars.Where(c => c.UserId == userId).ToListAsync();
    }

    public async Task<bool> UpdateCarAsync(Car car)
    {
        _context.Cars.Update(car);
        return (await _context.SaveChangesAsync()) > 0;
    }

    public async Task<bool> DeleteCarAsync(int carId)
    {
        var car = await _context.Cars.FindAsync(carId);
        if (car == null)
            return false;
        _context.Cars.Remove(car);
        return (await _context.SaveChangesAsync()) > 0;
    }
}