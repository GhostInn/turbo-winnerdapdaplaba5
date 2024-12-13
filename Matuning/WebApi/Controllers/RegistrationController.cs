using Matuning.Domain.Interfaces;
using Matuning.Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Matuning.WebApi.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserRegistrationService _registrationService;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(IUserRegistrationService registrationService, ILogger<RegistrationController> logger)
        {
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("user-with-car")]
        public async Task<IActionResult> RegisterUserWithCarAsync([FromBody] RegisterUserWithCarRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("RegisterUserWithCarAsync: request is null.");
                return BadRequest("Request body is required.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("RegisterUserWithCarAsync: invalid model state.");
                return BadRequest(ModelState);
            }

            try
            {
                var (Success, ErrorMessage) = await _registrationService.RegisterUserWithCarAsync(
                    request.Username, request.Email, request.PasswordHash, 
                    request.FirstName, request.LastName,
                    request.Make, request.Model, request.Year, request.EngineTypeId, request.TransmissionId);

                if (Success)
                {
                    _logger.LogInformation("User {Username} registered successfully with a car {Make} {Model}.", request.Username, request.Make, request.Model);
                    return Ok("User and car registered successfully.");
                }
                else
                {
                    _logger.LogWarning("RegisterUserWithCarAsync: registration failed. {ErrorMessage}", ErrorMessage);
                    return StatusCode(500, ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RegisterUserWithCarAsync: internal server error.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }
    }