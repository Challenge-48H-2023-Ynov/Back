using PartyPlanning.Data;

namespace PartyPlanning.Model.PartyModels;

public class PartyDTO
{
    public Guid IdParty { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Adress { get; set; }
    public string Maker { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
}

public static class PartyMapper
{
    public static PartyDTO ToDTO(this Party party)
    {
        return new PartyDTO
        {
            IdParty = party.IdParty,
            Name = party.Name,
            Description = party.Description,
            Adress = party.Adresse,
            Maker = party.User.UserName,
            DateStart = party.DateStart,
            DateEnd = party.DateFin
        };
    }

    public static List<PartyDTO> ToDTOList(this List<Party> parties)
    {
        return parties.Select(party => party.ToDTO()).ToList();
    }
}