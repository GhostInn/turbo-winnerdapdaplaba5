using Matuning.Domain.Models.Cars;
using Matuning.Domain.Models.Conversations;
using Matuning.Domain.Models.Notifications;
using Matuning.Domain.Models.Parts;
using Matuning.Domain.Models.Reports;
using Matuning.Domain.Models.Services;

namespace Matuning.Domain.Models.Users;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    
    // Навигационные свойства
    public ICollection<Car> Cars { get; set; }
    public ICollection<Part> Parts { get; set; }
    public ICollection<Service> Services { get; set; }
    public ICollection<Message> MessagesSent { get; set; }
    public ICollection<Report> Reports { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Notification> Notifications { get; set; }
}