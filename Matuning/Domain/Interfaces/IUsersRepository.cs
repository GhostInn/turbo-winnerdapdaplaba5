using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Interfaces;

public interface IUsersRepository
{
    Task<bool> AddUserAsync(User user);
    Task<User> GetUserByIdAsync(int userId);
    Task<User> GetUserByUsernameAsync(string username);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
}