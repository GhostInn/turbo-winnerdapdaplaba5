namespace Matuning.Domain.Models.Conversations;

public class Conversation
{
    public int ConversationId { get; set; }

    // Навигационные свойства
    public ICollection<Message> Messages { get; set; }
    public ICollection<ConversationParticipant> Participants { get; set; }
}