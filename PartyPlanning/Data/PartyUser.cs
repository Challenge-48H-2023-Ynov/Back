using Microsoft.AspNetCore.Identity;

namespace PartyPlanning.Data;

public class PartyUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string? Biography { get; set; }
    public bool HavePermis { get; set; }
    public string? Snap { get; set; }
    public string? Insta { get; set; }

    public ICollection<Participation>? Participations { get; set; }
    public ICollection<Party>? Parties { get; set; }
    public ICollection<Apport>? Apports { get; set; }
    public ICollection<Message>? Messages { get; set; }
    public ICollection<Invitation>? Invitations { get; set; }
}