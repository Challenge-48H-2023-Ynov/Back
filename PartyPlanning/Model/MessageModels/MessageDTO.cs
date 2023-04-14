using PartyPlanning.Data;

namespace PartyPlanning.Model.MessageModels;

public class MessageDTO
{
    public Guid IdMessage { get; set; }
    public string Content { get; set; }
    public DateTime SendDate { get; set; }

    public Guid IdParty { get; set; }
    public virtual Party? Party { get; set; }

    public Guid IdUser { get; set; }
    public virtual PartyUser? User { get; set; }
}

public static class MessageExtensions
{
    public static MessageDTO ToDTO(this Message message)
    {
        return new MessageDTO
        {
            IdMessage = message.IdMessage,
            Content = message.Content,
            SendDate = message.SendDate,
            IdParty = message.IdParty,
            Party = message.Party,
            IdUser = message.IdUser,
            User = message.User
        };
    }

    public static List<MessageDTO> ToDTOList(this List<Message> messages) => messages.Select(i => i.ToDTO()).ToList();
}
