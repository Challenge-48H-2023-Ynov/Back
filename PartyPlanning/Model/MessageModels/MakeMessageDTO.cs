using Microsoft.Build.Framework;
using PartyPlanning.Data;

namespace PartyPlanning.Model.MessageModels
{
    public class MakeMessageDTO
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid IdParty { get; set; }
        [Required]
        public Guid IdUser { get; set; }
    }
}
