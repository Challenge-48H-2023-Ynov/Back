using PartyPlanning.Data;
using System.ComponentModel.DataAnnotations;

namespace PartyPlanning.Model.AuthModels;

public class PartyUserDTO
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Email { get; set; }
}

public static class PartyUserMapper
{
    public static PartyUserDTO ToDTO(this PartyUser user)
    {
        return new PartyUserDTO()
        {
            UserName = user.UserName,
            Email = user.Email
        };
    }
}