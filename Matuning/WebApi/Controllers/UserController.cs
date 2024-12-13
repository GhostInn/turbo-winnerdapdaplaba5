using Matuning.Domain.Interfaces;
using Matuning.Domain.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matuning.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUsersRepository usersRepository, ILogger<UserController> logger)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        // Создание пользователя
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] User user)
        {
            if (user == null)
            {
                _logger.LogWarning("AddUserAsync: Пользовательская модель передана как null.");
                return BadRequest("User cannot be null.");
            }

            // Проверка валидности модели
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("AddUserAsync: Модель пользователя не валидна.");
                return BadRequest(ModelState);
            }

            try
            {
                bool result = await _usersRepository.AddUserAsync(user);
                if (result)
                {
                    // Предполагается, что UserId генерируется при создании
                    return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = user.UserId }, user);
                }
                else
                {
                    _logger.LogError("AddUserAsync: Не удалось добавить пользователя.");
                    return StatusCode(500, "An error occurred while creating the user.");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "AddUserAsync: Некорректные данные пользователя.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddUserAsync: Внутренняя ошибка сервера.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        
        // Получение пользователя по ID
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("GetUserByIdAsync: Некорректный userId {UserId}.", userId);
                return BadRequest("UserId must be a positive integer.");
            }

            try
            {
                var user = await _usersRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogInformation("GetUserByIdAsync: Пользователь с ID {UserId} не найден.", userId);
                    return NotFound($"User with ID {userId} was not found.");
                }

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "GetUserByIdAsync: Некорректный аргумент.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserByIdAsync: Внутренняя ошибка сервера.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        
        // Получение пользователя по имени пользователя
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("GetUserByUsernameAsync: Имя пользователя пусто или null.");
                return BadRequest("Username cannot be null or empty.");
            }

            try
            {
                var user = await _usersRepository.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    _logger.LogInformation("GetUserByUsernameAsync: Пользователь с именем {Username} не найден.", username);
                    return NotFound($"User with username '{username}' was not found.");
                }

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "GetUserByUsernameAsync: Некорректный аргумент.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserByUsernameAsync: Внутренняя ошибка сервера.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        
        // Обновление пользователя
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(int userId, [FromBody] User user)
        {
            if (user == null)
            {
                _logger.LogWarning("UpdateUserAsync: Пользовательская модель передана как null.");
                return BadRequest("User cannot be null.");
            }

            if (userId != user.UserId)
            {
                _logger.LogWarning("UpdateUserAsync: Несоответствие userId в URL и теле запроса. URL: {UserId}, Body: {BodyUserId}.", userId, user.UserId);
                return BadRequest("User ID in the URL does not match the User ID in the body.");
            }

            // Проверка валидности модели
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateUserAsync: Модель пользователя не валидна.");
                return BadRequest(ModelState);
            }

            try
            {
                bool result = await _usersRepository.UpdateUserAsync(user);

                if (result)
                {
                    return NoContent(); // 204 No Content означает успешное обновление без возвращаемого содержимого
                }
                else
                {
                    _logger.LogInformation("UpdateUserAsync: Пользователь с ID {UserId} не найден для обновления.", userId);
                    return NotFound($"User with ID {userId} was not found.");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "UpdateUserAsync: Некорректные данные пользователя.");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "UpdateUserAsync: Пользователь с ID {UserId} не найден.", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateUserAsync: Внутренняя ошибка сервера.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        
        // Удаление пользователя
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("DeleteUserAsync: Некорректный userId {UserId}.", userId);
                return BadRequest("UserId must be a positive integer.");
            }

            try
            {
                bool result = await _usersRepository.DeleteUserAsync(userId);

                if (result)
                {
                    return NoContent(); // 204 No Content означает успешное удаление
                }
                else
                {
                    _logger.LogInformation("DeleteUserAsync: Пользователь с ID {UserId} не найден для удаления.", userId);
                    return NotFound($"User with ID {userId} was not found.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "DeleteUserAsync: Пользователь с ID {UserId} не найден.", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteUserAsync: Внутренняя ошибка сервера.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        
        // Получение всех пользователей
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _usersRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllUsersAsync: Внутренняя ошибка сервера.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}