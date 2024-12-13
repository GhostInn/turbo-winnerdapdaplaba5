using Matuning.Domain.Enums;
using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Models.Notifications;

public class Notification
{
    public int NotificationId { get; set; }
    public int UserId { get; set; }
    public NotificationType NotificationType { get; set; }
    public int ReferenceId { get; set; } // Может ссылаться на part_id, service_id и т.д.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool ReadStatus { get; set; } = false;

    // Навигационные свойства
    public User User { get; set; }
}