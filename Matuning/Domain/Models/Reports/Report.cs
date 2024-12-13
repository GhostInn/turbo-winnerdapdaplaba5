using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Models.Reports;

public class Report
{
    public int ReportId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ContentUrl { get; set; } // Ссылка на фото или видео
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public User User { get; set; }
    public ICollection<Comment> Comments { get; set; }
}