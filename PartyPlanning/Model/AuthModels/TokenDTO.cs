using System.ComponentModel.DataAnnotations;

namespace PartyPlanning.Model.AuthModels;

public class TokenDTO
{
    public TokenDTO(string token, DateTime expirationDate)
    {
        Token = token;
        ExpirationDate = expirationDate;
    }

    public TokenDTO(string token)
    {
        Token = token;
        ExpirationDate = null;
    }

    [Required]
    public string Token { get; set; }

    public DateTime? ExpirationDate { get; set; }
}