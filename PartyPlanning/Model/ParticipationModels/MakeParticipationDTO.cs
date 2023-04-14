using System.ComponentModel.DataAnnotations;

namespace PartyPlanning.Model.ParticipationModels;

public class MakeParticipationDTO
{
    [Required]
    public Guid IdParty { get; set; }

    [Required]
    public Guid IdUser { get; set; }

    [Required]
    public int PlaceDispo { get; set; }

    [Required]
    public bool IsSAM { get; set; }

    [Required]
    public DateTime DateArrivee { get; set; }

    [Required]
    public DateTime DateDepart { get; set; }
}