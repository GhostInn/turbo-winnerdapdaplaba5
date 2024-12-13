using Matuning.Domain.Interfaces;
using Matuning.Domain.Models.Users;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Matuning.Infrastructure
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Проверка обязательных полей на null или пустоту
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username не может быть пустым или null.", nameof(user.Username));

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email не может быть пустым или null.", nameof(user.Email));

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentException("PasswordHash не может быть пустым или null.", nameof(user.PasswordHash));

            if (user.RegistrationDate == default)
                throw new ArgumentException("RegistrationDate не может быть пустым.", nameof(user.RegistrationDate));

            // Дополнительные проверки можно добавить по необходимости
            // Например, проверка формата Email

            _context.Users.Add(user);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId должен быть положительным числом.", nameof(userId));

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} was not found.");

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username не может быть пустым или null.", nameof(username));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                throw new KeyNotFoundException($"User with username '{username}' was not found.");

            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.UserId <= 0)
                throw new ArgumentException("Invalid UserId.", nameof(user.UserId));

            // Проверка обязательных полей на null или пустоту
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username не может быть пустым или null.", nameof(user.Username));

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email не может быть пустым или null.", nameof(user.Email));

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentException("PasswordHash не может быть пустым или null.", nameof(user.PasswordHash));

            if (user.RegistrationDate == default)
                throw new ArgumentException("RegistrationDate не может быть пустым.", nameof(user.RegistrationDate));

            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {user.UserId} был не найден.");

            // Обновление полей существующего пользователя
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.RegistrationDate = user.RegistrationDate;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Phone = user.Phone;

            _context.Users.Update(existingUser);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId должен быть положительным числом.", nameof(userId));

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} был не найден.");

            _context.Users.Remove(user);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
    }
}