using PartyPlanning.Data;

namespace PartyPlanning.Model.PartyModels;

public class PartyDTO
{
    public Guid IdParty { get; set; }
    public string Name { get; set; }
    public string Maker { get; set; }
}

public static class PartyMapper
{
    public static PartyDTO ToDTO(this Party party)
    {
        return new PartyDTO
        {
            IdParty = party.IdParty,
            Name = party.Name,
            Maker = party.User.UserName
        };
    }

    public static List<PartyDTO> ToDTOList(this List<Party> parties)
    {
        return parties.Select(party => party.ToDTO()).ToList();
    }
}
