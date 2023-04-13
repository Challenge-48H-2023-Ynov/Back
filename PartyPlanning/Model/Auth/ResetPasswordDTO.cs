using System.ComponentModel.DataAnnotations;

namespace PartyPlanning.Model.Auth
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmOAssword { get; set; }
    }
}