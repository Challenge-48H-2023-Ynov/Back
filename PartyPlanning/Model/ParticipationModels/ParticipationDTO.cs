using PartyPlanning.Data;

namespace PartyPlanning.Model.ParticipationModels;

public class ParticipationDTO
{
    public Guid IdParty { get; set; }
    public Guid IdUser { get; set; }
    public int PlaceDispo { get; set; }
    public bool IsSAM { get; set; }
}

public static class ParticipationExentionsion
{
    public static ParticipationDTO ToDTO(this Participation participation)
    {
        return new ParticipationDTO()
        {
            IdParty = participation.IdParty,
            IdUser = participation.IdUser,
            PlaceDispo = participation.PlaceDispo,
            IsSAM = participation.IsSAM,
        };
    }

    public static List<ParticipationDTO> ToDTOList(this List<Participation> participationList)
        => participationList.Select(p => p.ToDTO()).ToList();
}
