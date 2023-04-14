using PartyPlanning.Data;

namespace PartyPlanning.Model.InvitationModels;

public class InvitationDTO
{
    public Guid IdParty { get; set; }
    public string PartyName { get; set; }
    public string Username { get; set; }

}

public static class InvitationExtensions
{
    public static InvitationDTO ToDTO(this Invitation invitation)
    {
        return new InvitationDTO
        {
            IdParty = invitation.IdParty,
            PartyName = invitation.Party!.Name,
            Username = invitation.User!.UserName
        };
    }

    public static List<InvitationDTO> ToDTOList(this List<Invitation> invitations) => invitations.Select(i => i.ToDTO()).ToList();
}
