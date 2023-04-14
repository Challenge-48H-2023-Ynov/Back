using System.ComponentModel.DataAnnotations;

namespace PartyPlanning.Model.InvitationModels
{
    public class MakeInvitationDTO
    {
        [Required]
        public Guid IdParty { get; set; }
        [Required]
        public Guid IdUser { get; set; }
    }
}
