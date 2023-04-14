using System.ComponentModel.DataAnnotations;

namespace PartyPlanning.Model.AuthModels
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}