using Matuning.Domain.Models.Users;

namespace Matuning.Domain.Models.Conversations;

public class ConversationParticipant
{
    public int ConversationId { get; set; }
    public int UserId { get; set; }

    // Навигационные свойства
    public Conversation Conversation { get; set; }
    public User User { get; set; }
}