using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Models.Conversations;

public class Message
{
    public int MessageId { get; set; }
    public int ConversationId { get; set; }
    public int SenderUserId { get; set; }
    public string MessageText { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public Conversation Conversation { get; set; }
    public User Sender { get; set; }
}