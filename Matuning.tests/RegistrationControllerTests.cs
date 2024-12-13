using System.Net;
using Matuning.Domain.Interfaces;
using Matuning.Domain.Models.DTO;
using Matuning.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;

namespace Matuning.tests;

public class RegistrationControllerTests
{
    private readonly Mock<IUserRegistrationService> _mockService;
    private readonly Mock<ILogger<RegistrationController>> _mockLogger;
    private readonly RegistrationController _controller;

    public RegistrationControllerTests()
    {
        _mockService = new Mock<IUserRegistrationService>();
        _mockLogger = new Mock<ILogger<RegistrationController>>();
        _controller = new RegistrationController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task RegisterUserWithCarAsync_NullRequest_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.RegisterUserWithCarAsync(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Request body is required.", badRequestResult.Value);
    }

    [Fact]
    public async Task RegisterUserWithCarAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var invalidRequest = new RegisterUserWithCarRequest
        {
            // Не задаём обязательные поля, например Username, Email
            PasswordHash = "hash",
            FirstName = "John",
            LastName = "Doe",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2015,
            EngineTypeId = 1,
            TransmissionId = 1
        };

        // Добавляем искусственно ошибку валидации модели
        _controller.ModelState.AddModelError("Username", "Required");
        _controller.ModelState.AddModelError("Email", "Required");

        // Act
        var result = await _controller.RegisterUserWithCarAsync(invalidRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task RegisterUserWithCarAsync_ServiceReturnsSuccess_ReturnsOk()
    {
        // Arrange
        var validRequest = new RegisterUserWithCarRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash",
            FirstName = "John",
            LastName = "Doe",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2015,
            EngineTypeId = 1,
            TransmissionId = 1
        };

        // Сервис возвращает успех
        _mockService.Setup(s => s.RegisterUserWithCarAsync(
            validRequest.Username, validRequest.Email, validRequest.PasswordHash,
            validRequest.FirstName, validRequest.LastName,
            validRequest.Make, validRequest.Model, validRequest.Year, validRequest.EngineTypeId, validRequest.TransmissionId
        )).ReturnsAsync((true, (string)null));

        // Act
        var result = await _controller.RegisterUserWithCarAsync(validRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User and car registered successfully.", okResult.Value);
    }

    [Fact]
    public async Task RegisterUserWithCarAsync_ServiceReturnsError_Returns500()
    {
        // Arrange
        var validRequest = new RegisterUserWithCarRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash",
            FirstName = "John",
            LastName = "Doe",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2015,
            EngineTypeId = 1,
            TransmissionId = 1
        };

        // Сервис возвращает ошибку
        _mockService.Setup(s => s.RegisterUserWithCarAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<int>()
        )).ReturnsAsync((false, "Some error"));

        // Act
        var result = await _controller.RegisterUserWithCarAsync(validRequest);

        // Assert
        var objResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objResult.StatusCode);
        Assert.Equal("Some error", objResult.Value);
    }

    [Fact]
    public async Task RegisterUserWithCarAsync_ServiceThrowsException_Returns500()
    {
        // Arrange
        var validRequest = new RegisterUserWithCarRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash",
            FirstName = "John",
            LastName = "Doe",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2015,
            EngineTypeId = 1,
            TransmissionId = 1
        };

        // Сервис выбрасывает исключение
        _mockService.Setup(s => s.RegisterUserWithCarAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<int>()
        )).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.RegisterUserWithCarAsync(validRequest);

        // Assert
        var objResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objResult.StatusCode);
        Assert.Equal("Internal server error occurred.", objResult.Value);
    }
}