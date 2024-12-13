namespace Matuning.Domain.Interfaces;

public interface IUserRegistrationService
{
    Task<(bool Success, string ErrorMessage)> RegisterUserWithCarAsync(
        string username, string email, string passwordHash, 
        string firstName, string lastName, 
        string make, string model, short year, int engineTypeId, int transmissionId);
}