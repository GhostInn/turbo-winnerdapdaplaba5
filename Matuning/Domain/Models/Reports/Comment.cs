using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Models.Reports;

public class Comment
{
    public int CommentId { get; set; }
    public int ReportId { get; set; }
    public int UserId { get; set; }
    public string CommentText { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public Report Report { get; set; }
    public User User { get; set; }
}