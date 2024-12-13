using Matuning.Domain.Interfaces;
using Matuning.Domain.Models.Cars;
using Matuning.Domain.Models.Users;

namespace Matuning.Infrastructure.Services;

public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ICarsRepository _carsRepository;
        private readonly ApplicationDbContext _context;

        public UserRegistrationService(IUsersRepository usersRepository, ICarsRepository carsRepository, ApplicationDbContext context)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _carsRepository = carsRepository ?? throw new ArgumentNullException(nameof(carsRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterUserWithCarAsync(
            string username, string email, string passwordHash, 
            string firstName, string lastName, 
            string make, string model, short year, int engineTypeId, int transmissionId)
        {
            // Используем транзакцию для атомарности операции
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Создаем пользователя
                var newUser = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = passwordHash,
                    FirstName = firstName,
                    LastName = lastName,
                    RegistrationDate = DateTime.UtcNow
                };

                bool userCreated = await _usersRepository.AddUserAsync(newUser);
                if (!userCreated)
                {
                    await transaction.RollbackAsync();
                    return (false, "Failed to create user.");
                }

                // После сохранения у User будет UserId
                // Добавляем автомобиль
                var newCar = new Car
                {
                    UserId = newUser.UserId,
                    Make = make,
                    Model = model,
                    Year = year,
                    EngineTypeId = engineTypeId,
                    TransmissionId = transmissionId
                };

                var addedCar = await _carsRepository.AddCarAsync(newCar);
                if (addedCar == null)
                {
                    await transaction.RollbackAsync();
                    return (false, "Failed to add car for the user.");
                }

                await transaction.CommitAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"An error occurred: {ex.Message}");
            }
        }
    }