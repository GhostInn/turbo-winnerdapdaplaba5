namespace Matuning.Domain.Models.DTO;

    public class RegisterUserWithCarRequest
    {
        // Данные пользователя
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Данные автомобиля
        public string Make { get; set; } // Марка
        public string Model { get; set; } // Модель
        public short Year { get; set; }   // Год выпуска
        public int EngineTypeId { get; set; }
        public int TransmissionId { get; set; }
    }
