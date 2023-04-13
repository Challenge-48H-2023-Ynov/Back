using Microsoft.AspNetCore.Identity;

namespace PartyPlanning.Data;

public class PartyUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string Biography { get; set; }
    public bool HavePermis { get; set; }
    public string? Snap { get; set; }
    public string? Insta { get; set; }

    public ICollection<Party>? Parties { get; set; }
    public virtual Car? Car { get; set; }
    public ICollection<Apport>? apports { get; set; }
}