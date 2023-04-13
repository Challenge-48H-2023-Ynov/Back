using System.ComponentModel.DataAnnotations;

namespace PartyPlanning.Model.PartyModels
{
    public class MakePartyDTO
    {
        [Required]
        public Guid IdPartyMaker { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Adresse { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }

    }
}
